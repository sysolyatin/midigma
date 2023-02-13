using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Security.Cryptography;

namespace MidiToGrandMA.gma
{
    public class MA2Connector
    {
        public delegate void SessionCreatedHandler(MA2Connector o, EventArgs e);
        public delegate void ErrorCreatingSessionHandler(MA2Connector o, EventArgs e);
        public delegate void ErrorAuthenticatingHandler(MA2Connector o, EventArgs e);
        public delegate void AuthenticationSucceededHandler(MA2Connector o, EventArgs e);
        public delegate void CommandLEDHandler(MA2Connector o, CommandButtonLEDEventArgs e);
        public delegate void HistoryReceivedHandler(MA2Connector o, EventArgs e);
        public delegate void PromptChangedHandler(MA2Connector o, PromptChangedEventArgs e);
        public delegate void CommandConfirmationHandler(MA2Connector o, CommandConfirmationEventArgs e);
        public delegate void ConnectionLostHandler(MA2Connector o, EventArgs e);

        public WebSocket webSocket;

        private bool FIsConnectionEstablished;
        private bool FIsAuthenticated;
        private readonly Dictionary<string, Action<dynamic>> FHandlers = new Dictionary<string, Action<object>>();

        private readonly BackgroundWorker FWorker = new BackgroundWorker();
        private readonly Stopwatch Timer1 = new Stopwatch();
        private readonly Stopwatch Timer2 = new Stopwatch();
        private readonly Stopwatch Timer3 = new Stopwatch();
        private readonly Stopwatch Timer4 = new Stopwatch();

        public MA2Playbacks Playbacks { get; private set; }
        //public MA2Command Command { get; private set; }

        public int SessionNumber { get; private set; }
        public bool IsConnected { get; private set; }

        private readonly MaEncoderQueue ButtonQueue = new MaEncoderQueue();
        private readonly BackgroundWorker EncoderWorker = new BackgroundWorker();
        private readonly Dictionary<string, bool> StateList = new Dictionary<string, bool>();
        private readonly List<EncoderWheelState> EncoderWheelStateList = new List<EncoderWheelState>();
        private readonly object FCommandLineLock = new object();
        private readonly List<string> FCommandLineHistory = new List<string>();
        private string Password = "";
        private readonly Stopwatch TimerGetData = new Stopwatch();
        private readonly object TimerGetDataLock = new object();
        private readonly Stopwatch PresetTimer = new Stopwatch();
        private readonly object PresetLock = new object();
        private readonly Dictionary<string, int> ExecutorList = new Dictionary<string, int>();
        public List<string> CommandLineHistory
        {
            get
            {
                lock (FCommandLineLock)
                {
                    return FCommandLineHistory;
                }
            }
        }
        public string PromptText { get; private set; } = "";

        public event SessionCreatedHandler SessionCreated;
        public event ErrorCreatingSessionHandler ErrorCreatingSession;
        public event AuthenticationSucceededHandler AuthenticationSucceeded;
        public event CommandLEDHandler OnLEDChanged;
        public event HistoryReceivedHandler OnHistoryReceived;
        public event PromptChangedHandler OnPromptChanged;
        public event CommandConfirmationHandler OnCommandConfirmation;
        public event ConnectionLostHandler OnConnectionLost;

        private string _gmaLogin;
        private string _gmaPassword;

        public MA2Connector(string gmaHost, string gmaUser, string gmaPassword)
        {
            Playbacks = new MA2Playbacks(this);
            _gmaLogin = gmaUser;
            _gmaPassword = gmaPassword;

            FHandlers["playbacks"] = Playbacks.HandleType;
            FHandlers["login"] = OnLoginHandler;
            FHandlers["getdata"] = CommandHandleType;
            FHandlers["commandHistory"] = CommandHandleType;
            FHandlers["presetTypes"] = CommandHandleType;
            FHandlers["commandConfirmation"] = HandleCommandConfirmation;

            webSocket = (WebSocket)(object)new WebSocket("ws://" + gmaHost + "/?ma=1", Array.Empty<string>());
            webSocket.Compression = CompressionMethod.Deflate;
            webSocket.OnOpen += OnOpenHandler;
            webSocket.OnClose += OnCloseHandler;
            webSocket.OnMessage += OnMessageHandler;
            webSocket.OnError += OnErrorHandler;
            webSocket.Connect();

            if (!IsConnected)
            {
                throw new Exception("Could not connect (check compatible version / credentials)");
            }

            Playbacks.StartDispatcher();
            FWorker.DoWork += WorkerJob;
            FWorker.RunWorkerAsync();

            for (uint num = 0u; num < 4; num++)
            {
                EncoderWheelStateList.Add(new EncoderWheelState());
            }
            EncoderWorker.DoWork += EncoderThread;
            EncoderWorker.RunWorkerAsync();
        }


        private void WorkerJob(object P_0, DoWorkEventArgs P_1)
        {
            while (IsConnected)
            {
                Thread.Sleep(10);
                ThreadJob(P_0, P_1);
            }
        }

        private void OnLoginHandler(dynamic P_0)
        {
            if (!((P_0["result"] != null) ? true : false))
            {
                return;
            }
            if (P_0["result"] == false)
            {
                ErrorCreatingSession?.Invoke(this, EventArgs.Empty);
                return;
            }
            FIsAuthenticated = true;
            AuthenticationSucceeded?.Invoke(this, EventArgs.Empty);
        }

        public void Disconnect()
        {
            try
            {
                Console.WriteLine("Connector DISCONNECT");
                FIsConnectionEstablished = false;
                IsConnected = false;
                webSocket.CloseAsync();
            }
            catch
            {
            }
        }

        private void ThreadJob(object P_0, EventArgs P_1)
        {
            if (FIsConnectionEstablished && SessionNumber != 0 && IsConnected && FIsAuthenticated)
            {
                try
                {
                    if (!Timer1.IsRunning || Timer1.ElapsedMilliseconds > 10000L)
                    {
                        webSocket.Send("{\"session\":" + SessionNumber + "}");
                        Timer1.Restart();
                    }
                    if (Timer3.IsRunning && Timer3.ElapsedMilliseconds < 300L)
                    {
                        if (Timer4.IsRunning && Timer4.ElapsedMilliseconds < 66L)
                        {
                            if (!Timer2.IsRunning || Timer2.ElapsedMilliseconds > 33L)
                            {
                                Playbacks.Update();
                                Timer2.Restart();
                            }
                        }
                        else
                        {
                            UpdatePresetTypes();
                            Timer4.Restart();
                        }
                    }
                    else
                    {
                        UpdateCommand();
                        Timer3.Restart();
                    }
                }
                catch
                {
                }
            }
        }

        public void RefreshControls()
        {
            StateListClear();
            Playbacks.ClearAll();
        }

        private void OnMessageHandler(object sender, MessageEventArgs msg)
        {
            if (msg.Data.Contains("\"status\":\"server ready\", \"appType\":\"gma2\""))
            {
                NewSessionRequest();
            }
            else
            {
                try
                {
                    dynamic val = JsonConvert.DeserializeObject<object>(msg.Data);
                    try
                    {
                        if (val["session"] != null)
                        {
                            int num = (int)val["session"].Value;
                            if (num >= 0)
                            {
                                if (num == 0)
                                {
                                    NewSessionRequest();
                                }
                                else if (SessionNumber != num)
                                {
                                    Console.WriteLine("Session number: " + num);
                                    SessionNumber = num;
                                    SessionCreated?.Invoke(this, null);
                                    FIsConnectionEstablished = true;
                                    LoginRequest();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Remotes not enabled!");
                                ErrorCreatingSession?.Invoke(this, null);
                            }
                            SessionNumber = num;
                        }
                    }
                    catch
                    {
                    }
                    if (val["responseType"] != null)
                    {
                        string text = (string)val.responseType.Value;
                        if (text != "getdata" && text != "commandHistory" && text != "presetTypes" && text != "playbacks")
                        {
                            Console.WriteLine("responseType: " + text);
                        }
                        if (FHandlers.ContainsKey(text))
                        {
                            FHandlers[text](val);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Hello...");
                }
            }
        }

        private void LoginRequest()
        {
            string text = MD5Hash(_gmaPassword);
            string text2 = "{\"requestType\":\"login\",\"username\":\"" + _gmaLogin + "\",\"password\":\"" + text + "\",\"session\":" + SessionNumber + ",\"maxRequests\":10}";
            webSocket.Send(text2);
        }

        public static string MD5Hash(string input)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        private void OnCloseHandler(object P_0, CloseEventArgs P_1)
        {
            IsConnected = false;
            if (FIsConnectionEstablished)
            {
                Console.WriteLine("WS Disconnected...retrying...");
                webSocket.Connect();
            }
            else
            {
                OnConnectionLost?.Invoke(this, new EventArgs());
            }
        }

        private void OnOpenHandler(object P_0, EventArgs P_1)
        {
            IsConnected = true;
            Console.WriteLine("WS connected");
        }

        private void OnErrorHandler(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("WS Error: " + e.Message);
        }

        private void NewSessionRequest()
        {
            Console.WriteLine("New Session Request");
            webSocket.Send("{\"session\":0}");
        }

        private void TerminateSession(int P_0)
        {
            Console.WriteLine("Closing session...");
            webSocket.Send("{\"session\": " + P_0 + "}");
        }


        private void EncoderThread(object P_0, DoWorkEventArgs P_1)
        {
            while (true)
            {
                while (ButtonQueue.Count == 0)
                {
                    Thread.Sleep(30);
                }
                Thread.Sleep(10);
                MA2ButtonQueue data = ButtonQueue.Dequeue();
                if (data != null)
                {
                    ProcessButton(data.Executor, data.State);
                }
            }
        }

        public void HandlePrompt(dynamic reply)
        {
            if (reply["prompt"] != null)
            {
                try
                {
                    string text = (string)reply["prompt"];
                    if (text != PromptText)
                    {
                        OnPromptChanged?.Invoke(this, new PromptChangedEventArgs
                        {
                            lastPromptText = PromptText,
                            newPromptText = text
                        });
                        PromptText = text;
                    }
                }
                catch
                {
                }
            }
        }

        public void HandleCommandConfirmation(dynamic reply)
        {
            bool flag;
            if (!((reply["responseType"] != null && reply["responseType"] == "commandConfirmation") ? true : false) || !((((!(flag = (OnCommandConfirmation != null))) ? ((object)flag) : (flag & (reply["tt"] != null))) && reply["msg"] != null && reply["focusedButtonIndex"] != null && reply["buttons"] != null) ? true : false))
            {
                return;
            }
            string title = (string)reply["tt"];
            string message = (string)reply["msg"];
            dynamic val = reply["buttons"];
            int num = val.Count;
            if (num > 0)
            {
                List<string> list = new List<string>(num);
                ExecutorList.Clear();
                for (int i = 0; i < num; i++)
                {
                    string text = (string)val[i]["text"];
                    text = text.Replace("|", " ");
                    list.Add(text);
                    ExecutorList[text] = (int)val[i]["id"];
                }
                int index = 0;
                OnCommandConfirmation?.Invoke(this, new CommandConfirmationEventArgs
                {
                    Title = title,
                    Message = message,
                    Options = list,
                    DefaultOption = list[index]
                });
            }
        }

        public void CancelCommandConfirmation()
        {
            string str = "{\"requestType\":\"commandConfirmationResult\",";
            str += "\"result\":-1,";
            str = str + "\"session\":" + SessionNumber + ", \"maxRequests\": 0}";
            webSocket.Send(str);
        }

        public void SendCommandConfirmation(string optionSelected)
        {
            if (!ExecutorList.ContainsKey(optionSelected))
            {
                CancelCommandConfirmation();
                return;
            }
            string text = "{\"requestType\":\"commandConfirmationResult\",";
            text = text + "\"result\":" + ExecutorList[optionSelected] + ",";
            text = text + "\"session\":" + SessionNumber + ", \"maxRequests\": 0}";
            webSocket.Send(text);
        }

        public void CommandHandleType(dynamic reply)
        {
            bool flag2;
            if (reply["responseType"] != null && reply.responseType == "getdata")
            {
                dynamic val = reply["data"];
                if (val != null)
                {
                    int num = val.Count;
                    for (int i = 0; i < num; i++)
                    {
                        try
                        {
                            dynamic val2 = reply["data"][i];
                            foreach (object item in val2.Properties())
                            {
                                JProperty obj = (JProperty)(dynamic)item;
                                string name = obj.Name;
                                bool flag = (string)obj.Value != "0";
                                if (OnLEDChanged != null && (!StateList.ContainsKey(name) || (StateList.ContainsKey(name) && StateList[name] != flag)))
                                {
                                    OnLEDChanged(this, new CommandButtonLEDEventArgs
                                    {
                                        keyname = name,
                                        value = flag
                                    });
                                }
                                StateList[name] = flag;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                lock (TimerGetDataLock)
                {
                    TimerGetData.Stop();
                }
            }
            else if (((!(flag2 = (OnHistoryReceived != null))) ? ((object)flag2) : (flag2 & (reply.responseType != null))) && reply.responseType == "commandHistory")
            {
                try
                {
                    dynamic val3 = reply["cols"];
                    dynamic val4 = reply["data"];
                    if (val3 != null && val4 != null)
                    {
                        int num2 = -1;
                        int num3 = 0;
                        foreach (dynamic item2 in val3)
                        {
                            if (item2.Value == "Command")
                            {
                                num2 = num3;
                            }
                            num3++;
                        }
                        if (num2 != -1)
                        {
                            bool flag3 = false;
                            lock (FCommandLineLock)
                            {
                                string text = "";
                                FCommandLineHistory.Clear();
                                foreach (dynamic item3 in val4)
                                {
                                    string text2 = item3[num2].Value;
                                    FCommandLineHistory.Insert(0, text2);
                                    text += MA2Connector.MD5Hash(text2);
                                }
                                text = MA2Connector.MD5Hash(text);
                                flag3 = (text != Password);
                                Password = text;
                            }
                            if (flag3)
                            {
                                OnHistoryReceived(this, null);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            else
            {
                if (!((reply["responseType"] != null && reply["responseType"] == "presetTypes") ? true : false))
                {
                    return;
                }
                bool flag4 = false;
                dynamic val5 = reply["pre"];
                if (val5 != null)
                {
                    int num4 = val5.Count;
                    for (int j = 0; j < num4; j++)
                    {
                        dynamic val6 = val5[j];
                        dynamic val7 = val6["s"];
                        if (!((val7 != null && val7 == true) ? true : false))
                        {
                            continue;
                        }
                        dynamic val8 = val6["fea"];
                        if (!((val8 != null) ? true : false))
                        {
                            continue;
                        }
                        int num5 = val8.Count;
                        for (int k = 0; k < num5; k++)
                        {
                            dynamic val9 = val8[k];
                            dynamic val10 = val9["s"];
                            if (!((val10 != null && val10 == true) ? true : false))
                            {
                                continue;
                            }
                            dynamic val11 = val9["att"];
                            if (!((val11 != null) ? true : false))
                            {
                                continue;
                            }
                            int num6 = val11.Count;
                            for (int l = 0; l < 4; l++)
                            {
                                if (l >= num6)
                                {
                                    EncoderWheelStateList[l].attribute = null;
                                    EncoderWheelStateList[l].resolution = 0;
                                    EncoderWheelStateList[l].scrollMult = 1f;
                                    continue;
                                }
                                dynamic val12 = val11[l];
                                dynamic val13 = val12["n"] == null && val12["np"] == null;
                                if (!(val13 ? true : false) && !((val13 | (val12["encoder_resolution"] == null)) ? true : false))
                                {
                                    if (!((val12["n"] != null) ? true : false))
                                    {
                                        EncoderWheelStateList[l].attribute = val12["np"];
                                    }
                                    else
                                    {
                                        EncoderWheelStateList[l].attribute = val12["n"];
                                    }
                                    EncoderWheelStateList[l].resolution = val12["encoder_resolution"];
                                    switch (EncoderWheelStateList[l].resolution)
                                    {
                                        case 2:
                                            EncoderWheelStateList[l].scrollMult = 0.5f;
                                            break;
                                        default:
                                            EncoderWheelStateList[l].scrollMult = 1f;
                                            break;
                                        case 1:
                                            EncoderWheelStateList[l].scrollMult = 0.75f;
                                            break;
                                    }
                                    flag4 = true;
                                }
                                else
                                {
                                    EncoderWheelStateList[l].attribute = null;
                                    EncoderWheelStateList[l].resolution = 0;
                                    EncoderWheelStateList[l].scrollMult = 1f;
                                }
                            }
                        }
                    }
                }
                if (!flag4)
                {
                    EncoderWheelStateList[0].attribute = null;
                    EncoderWheelStateList[1].attribute = null;
                    EncoderWheelStateList[2].attribute = null;
                    EncoderWheelStateList[3].attribute = null;
                }
                lock (PresetLock)
                {
                    PresetTimer.Stop();
                }
            }
        }

        internal void StateListClear()
        {
            StateList.Clear();
        }

        public void UpdateCommand()
        {
            lock (TimerGetDataLock)
            {
                if (!TimerGetData.IsRunning || TimerGetData.ElapsedMilliseconds >= 1000L)
                {
                    string str = "{\"requestType\":\"getdata\",";
                    str += "\"data\":\"set,on,off,select,align,assign,copy,delete,time,goto,sequence,cue,exec,macro,page,group,preset,edit,update,store,clear,solo,high\",";
                    str = str + "\"session\":" + SessionNumber + ", \"maxRequests\": 1}";
                    webSocket.Send(str);
                }
            }
        }

        public void UpdatePresetTypes()
        {
            lock (PresetLock)
            {
                if (!PresetTimer.IsRunning || PresetTimer.ElapsedMilliseconds >= 1000L)
                {
                    string text = new JObject
                    {
                        ["requestType"] = "presetTypes",
                        ["type"] = "full",
                        ["session"] = SessionNumber,
                        ["maxRequests"] = 1
                    }.ToString(Formatting.None);
                    webSocket.Send(text);
                }
            }
        }

        public bool GetButtonLED(string key)
        {
            if (StateList.ContainsKey(key))
            {
                return StateList[key];
            }
            return false;
        }

        public void SetCommandButtonState(string key, bool pressed)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ButtonQueue.Enqueue(new MA2ButtonQueue
                {
                    Executor = key,
                    State = pressed
                });
            }
        }

        public void SetCommandButtonState(MA2KeyParam key, bool press)
        {
            SetCommandButtonState(MA2Enums.KeyEnumToCode(key), press);
        }

        private void ProcessButton(string P_0, bool P_1)
        {
            string str = "{\"requestType\":\"keyname\",";
            str = str + "\"keyname\": \"" + P_0 + "\",";
            str = str + "\"value\":" + (P_1 ? 1 : 0) + ",";
            str = str + "\"session\":" + SessionNumber + ",";
            str += "\"maxRequests\":0}";
            webSocket.Send(str);
        }

        public void Execute(string command)
        {
            string text = new JObject
            {
                ["requestType"] = "command",
                ["command"] = command,
                ["session"] = SessionNumber,
                ["maxRequests"] = 0
            }.ToString(Formatting.None);
            webSocket.Send(text);
        }

        public void ScrollEncoder(string attribute, float scroll_amount)
        {
            string str = "{\"requestType\":\"encoder\",";
            str = str + "\"name\":\"" + attribute + "\",";
            str = str + "\"value\":" + scroll_amount.ToString(CultureInfo.InvariantCulture) + ",";
            str = str + "\"session\":" + SessionNumber + ",";
            str += "\"maxRequests\":0}";
            webSocket.Send(str);
        }

        public void WheelRotate(int wheelNumber, int scroll_amount)
        {
            Execute("LUA 'gma.canbus.encoder(" + (wheelNumber - 1) + "," + scroll_amount + ",nil)'");
        }

        public void WheelButton(int wheelNumber, bool press)
        {
            Execute("LUA 'gma.canbus.encoder(" + (wheelNumber - 1) + ",0," + (press ? "true" : "false") + ")'");
        }

        public void TouchFader(int executorNumber, bool touched)
        {
            Execute("LUA 'gma.canbus.fader(" + (executorNumber - 1) + ", -1, " + (touched ? "true" : "false") + ", false)'");
        }

        public void SetHardkey(MA2Hardkeys key, bool pressed)
        {
            Execute("LUA 'gma.canbus.hardkey(" + (int)key + ", " + (pressed ? "true" : "false") + ", false)'");
        }

        public void ScrollEncoder(int wheelNumber, float scroll_amount)
        {
            EncoderWheelState currentEncoderState = GetCurrentEncoderState(wheelNumber);
            if (currentEncoderState != null)
            {
                string attribute = currentEncoderState.attribute;
                if (attribute != null)
                {
                    float scroll_amount2 = scroll_amount * currentEncoderState.scrollMult;
                    ScrollEncoder(attribute, scroll_amount2);
                }
            }
        }

        public EncoderWheelState GetCurrentEncoderState(int wheelNumber)
        {
            if (wheelNumber >= 1 && wheelNumber <= 4)
            {
                return EncoderWheelStateList[wheelNumber - 1];
            }
            return null;
        }
    }
}

