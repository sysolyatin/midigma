using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MidiToGrandMA.gma
{
    public class MA2Playbacks
    {
        //public delegate void ExecutorFaderMovedHandler(MA2Playbacks o, MsgFeedbackMA2Executor e);
        public delegate void ExecutorChangedHandler(MA2Playbacks o, MsgFeedbackMA2Executor e);

        private readonly MA2Connector _Connector;
        private int _ButtonPage = 1;
        private int _FaderPage = 1;

        private readonly Stopwatch Timer = new Stopwatch();
        private readonly object _Lock = new object();

        private readonly MaDataQueue executorData = new MaDataQueue();
        private readonly BackgroundWorker _BackgroundWorker = new BackgroundWorker();
        public readonly Dictionary<int, MA2Executor> ExecutorData = new Dictionary<int, MA2Executor>();
        public readonly Dictionary<int, MA2Executor> ExecutorFixed = new Dictionary<int, MA2Executor>();
        public int FadersPageNumber
        {
            get => _FaderPage;
            set
            {
                lock (_Lock)
                {
                    if (value >= 1)
                    {
                        _FaderPage = value;
                    }
                    _Connector.Execute("FaderPage " + _FaderPage);
                }
            }
        }
        public int ButtonsPageNumber
        {
            get => _ButtonPage;
            set
            {
                lock (_Lock)
                {
                    if (value >= 1)
                    {
                        _ButtonPage = value;
                    }
                    _Connector.Execute("ButtonPage " + _ButtonPage);
                }
            }
        }

        public int FixedButtonsPageNumber { get; set; }

        //public event ExecutorFaderMovedHandler ExecutorFaderMoved;
        public event ExecutorChangedHandler ExecutorChanged;

        public MA2Playbacks(MA2Connector Conn) => _Connector = Conn;

        public void StartDispatcher()
        {
            _BackgroundWorker.DoWork += WorkerJob;
            _BackgroundWorker.RunWorkerAsync();
        }

        private void WorkerJob(object P_0, DoWorkEventArgs P_1)
        {
            while (true)
            {
                while (executorData.Count == 0)
                {
                    Thread.Sleep(30);
                }
                Thread.Sleep(5);
                ExecutorData Data = executorData.Dequeue2();
                if (_Connector != null && Data != null && _Connector.IsConnected)
                {
                    ApplyExecutorData(Data.ExecNo, Data.Value, Data.PageNo);
                }
            }
        }

        public void HandleType(dynamic root)
        {
            foreach (dynamic item in root.itemGroups)
            {
                //Fader Data
                if (item["itemsType"] != null && item["itemsType"] == 2)
                {
                    int fadersPageNumber = FadersPageNumber;
                    if (item["iPage"] != null)
                    {
                        int.TryParse(item["iPage"], out fadersPageNumber);
                    }
                    if (item["items"] != null)
                    {
                        dynamic val = item["items"];
                        int num = val.Count;
                        for (int i = 0; i < num; i++)
                        {
                            dynamic val2 = val[i];
                            int num2 = val2.Count;
                            for (int j = 0; j < num2; j++)
                            {
                                dynamic val3 = val2[j];
                                try
                                {
                                    int iExecValue = (int)val3.iExec.Value;
                                    int combinedItemsCnt = (int)val3.combinedItems;
                                    for (int k = 0; k < combinedItemsCnt; k++)
                                    {
                                        float faderValue = (float)val3.executorBlocks[k].fader.v.Value;
                                        int key = iExecValue + k + 1;

                                        if (!ExecutorData.ContainsKey(key))
                                            ExecutorData[key] = new MA2Executor();
                                        MA2Executor Executor = ExecutorData[key];
                                        if (val3.executorBlocks[k].fader.tt != null)
                                        {
                                            Executor.FaderPriority = (string)val3.executorBlocks[k].fader.tt.Value;
                                        }
                                        else
                                        {
                                            Executor.FaderPriority = "";
                                        }
                                        if (((Newtonsoft.Json.Linq.JContainer)val3.executorBlocks[k]).Count > 3)
                                        {
                                            Executor.Button1 = val3.executorBlocks[k].button1.t;
                                            Executor.Button2 = val3.executorBlocks[k].button2.t;
                                            Executor.Button3 = val3.executorBlocks[k].button3.t;
                                        }
                                        bool IsChanged = false;

                                        string execColor = "#FFFFFF";
                                        execColor = val3["bdC"];
                                        if (Executor.Appearance != execColor)
                                        {
                                            IsChanged = true;
                                            Executor.Appearance = execColor;
                                        }

                                        string execLabel = "";
                                        execLabel = val3["tt"]["t"];
                                        if (Executor.Label != execLabel)
                                        {
                                            IsChanged = true;
                                        }
                                        Executor.Label = execLabel;


                                        string PrevCueName = "";
                                        string CueName = "";
                                        string NextCueName = "";
                                        float CueProgress = -1f;
                                        float PrevCueProgress = -1f;
                                        float NextCueProgress = -1f;
                                        if (val3["cues"] != null && val3["cues"]["items"] != null)
                                        {
                                            try
                                            {
                                                JToken jToken = val3["cues"]["items"];
                                                switch (jToken.Count())
                                                {
                                                    case 3:
                                                        PrevCueName = jToken[0]["t"]?.ToString();
                                                        CueName = jToken[1]["t"]?.ToString();
                                                        NextCueName = jToken[2]["t"]?.ToString();
                                                        if (jToken[0]["pgs"] != null && jToken[0]["pgs"]["v"] != null)
                                                        {
                                                            PrevCueProgress = (float)jToken[0]["pgs"]["v"];
                                                        }
                                                        if (jToken[1]["pgs"] != null && jToken[1]["pgs"]["v"] != null)
                                                        {
                                                            CueProgress = (float)jToken[1]["pgs"]["v"];
                                                        }
                                                        if (jToken[2]["pgs"] != null && jToken[2]["pgs"]["v"] != null)
                                                        {
                                                            NextCueProgress = (float)jToken[2]["pgs"]["v"];
                                                        }

                                                        if (Executor.PrevCue != PrevCueName || Executor.CurrCue != CueName || Executor.NextCue != NextCueName || Executor.PrevCueProgress != PrevCueProgress || Executor.CurrCueProgress != CueProgress || Executor.NextCueProgress != NextCueProgress)
                                                        {
                                                            IsChanged = true;
                                                        }

                                                        Executor.PrevCue = PrevCueName;
                                                        Executor.CurrCue = CueName;
                                                        Executor.NextCue = NextCueName;

                                                        Executor.PrevCueProgress = jToken[0]["pgs"] != null && jToken[0]["pgs"]["v"] != null ? PrevCueProgress : -1f;
                                                        Executor.CurrCueProgress = jToken[1]["pgs"] != null && jToken[1]["pgs"]["v"] != null ? CueProgress : -1f;
                                                        Executor.NextCueProgress = jToken[2]["pgs"] != null && jToken[2]["pgs"]["v"] != null ? NextCueProgress : -1f;
                                                        break;
                                                    default:
                                                        PrevCueName = null;
                                                        CueName = null;
                                                        NextCueName = null;
                                                        if (Executor.PrevCue != PrevCueName || Executor.CurrCue != CueName || Executor.NextCue != NextCueName || Executor.CurrCueProgress != CueProgress)
                                                        {
                                                            IsChanged = true;
                                                        }
                                                        Executor.PrevCue = PrevCueName;
                                                        Executor.CurrCue = CueName;
                                                        Executor.NextCue = NextCueName;
                                                        Executor.CurrCueProgress = -1f;
                                                        break;
                                                    case 1:
                                                        PrevCueName = null;
                                                        CueName = jToken[0]["t"]?.ToString();
                                                        NextCueName = null;
                                                        if (jToken[0]["pgs"] != null && jToken[0]["pgs"]["v"] != null)
                                                        {
                                                            CueProgress = (float)jToken[0]["pgs"]["v"];
                                                        }
                                                        if (Executor.PrevCue != PrevCueName || Executor.CurrCue != CueName || Executor.NextCue != NextCueName || Executor.CurrCueProgress != CueProgress)
                                                        {
                                                            IsChanged = true;
                                                        }
                                                        Executor.PrevCue = PrevCueName;
                                                        Executor.CurrCue = CueName;
                                                        Executor.NextCue = NextCueName;
                                                        Executor.CurrCueProgress = jToken[0]["pgs"] != null && jToken[0]["pgs"]["v"] != null ? CueProgress : -1f;
                                                        Executor.PrevCueProgress = -1f;
                                                        Executor.NextCueProgress = -1f;
                                                        break;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message + "!13");
                                            }
                                        }
                                        else
                                        {
                                            Executor.PrevCue = null;
                                            Executor.CurrCue = null;
                                            Executor.NextCue = null;
                                            Executor.CurrCueProgress = -1f;
                                        }


                                        bool IsAssigned = true;
                                        try
                                        {
                                            JToken jToken2 = val3.SelectToken("$.executorBlocks[" + k + "].button1.t");
                                            IsAssigned = (jToken2 != null && jToken2.ToString() != "Empty");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message + "!14");
                                        }
                                        Executor.IsEmpty = !IsAssigned;

                                        if (Executor.FaderValue != faderValue)
                                        {
                                            IsChanged = true;
                                            Executor.FaderValue = faderValue;
                                        }


                                        ExecutorState execState = ExecutorState.Unassigned;
                                        if (IsAssigned)
                                        {
                                            execState = ExecutorState.Off;
                                            if (val3["isRun"] != null && (int)val3["isRun"] != 0)
                                            {
                                                execState = ExecutorState.On;
                                            }
                                        }
                                        if (execState != Executor.State)
                                        {
                                            IsChanged = true;
                                            Executor.State = execState;
                                        }

                                        if ((ExecutorChanged != null) && (IsChanged))
                                        {
                                            ExecutorChanged(this, new MsgFeedbackMA2Executor
                                            {
                                                ExecutorPage = fadersPageNumber + 1,
                                                ExecutorNumber = iExecValue + k + 1,
                                                IsButton = true,
                                                Executor = Executor,
                                                LEDOn = (IsAssigned && execState == ExecutorState.On),
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "!15");
                                }
                            }
                        }
                    }
                }


                //Button Data
                if (item["itemsType"] != null && item["itemsType"] == 3)
                {
                    int buttonsPageNumber = ButtonsPageNumber;
                    if (item["iPage"] != null)
                    {
                        int.TryParse(item.iPage, out buttonsPageNumber);
                    }
                    if (item["items"] != null)
                    {
                        int num9 = item["items"].Count;
                        for (int l = 0; l < num9; l++)
                        {
                            dynamic val4 = item["items"][l];
                            int num10 = val4.Count;
                            for (int m = 0; m < num10; m++)
                            {
                                bool IsChanged = false;
                                dynamic val5 = val4[m];
                                int num11 = -1;
                                try
                                {
                                    num11 = (int)val5.iExec.Value;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message + "!16");
                                }

                                int num12 = (int)val5.combinedItems;
                                for (int n = 0; n < num12; n++)
                                {
                                    int key2 = num11 + n;
                                    MA2Executor Executor;
                                    if ((buttonsPageNumber != ButtonsPageNumber) && (buttonsPageNumber == FixedButtonsPageNumber))
                                    {
                                        if (!ExecutorFixed.ContainsKey(key2))
                                            ExecutorFixed[key2] = new MA2Executor();
                                        Executor = ExecutorFixed[key2];
                                    }
                                    else
                                    {
                                        if (!ExecutorData.ContainsKey(key2))
                                            ExecutorData[key2] = new MA2Executor();
                                        Executor = ExecutorData[key2];
                                    }
                                    ExecutorState State = ExecutorState.Unassigned;
                                    bool IsAssigned = true;
                                    try
                                    {
                                        JToken jToken3 = val5.SelectToken("$.isRun");
                                        if (jToken3 != null && jToken3.ToString() == "true")
                                        {
                                            IsAssigned = true;
                                        }
                                        else
                                        {
                                            JToken jToken4 = val5.SelectToken("$.tt.t").ToString();
                                            IsAssigned = (jToken4 != null && !string.IsNullOrEmpty(jToken4.ToString()));
                                        }
                                        int width = 0;
                                        int.TryParse(val5.SelectToken("combinedItems").ToString(), out width);
                                        Executor.Width = width;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "!1");
                                    }
                                    Executor.IsEmpty = IsAssigned;


                                    if (IsAssigned)
                                    {
                                        try
                                        {
                                            State = (((int)val5["isRun"].Value != 0) ? ExecutorState.On : ExecutorState.Off);
                                            if (State != Executor.State)
                                            {
                                                IsChanged = true;
                                                Executor.State = State;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message + "!2");
                                        }
                                    }

                                    string Color = "#FFFFFF";
                                    try
                                    {
                                        Color = val5["bdC"];
                                        if (Executor.Appearance != Color)
                                        {
                                            IsChanged = true;
                                            Executor.Appearance = Color;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "!3");
                                    }

                                    string Label = "";
                                    try
                                    {
                                        Label = val5["tt"]["t"];
                                        if (Executor.Label != Label)
                                        {
                                            IsChanged = true;
                                            Executor.Label = Label;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + "!4");
                                    }

                                    string PrevCue = "";
                                    string CurrCue = "";
                                    string NextCue = "";
                                    float PrevCueProgress = -1f;
                                    float CurrCueProgress = -1f;
                                    float NextCueProgress = -1f;
                                    if (val5["cues"] != null && val5["cues"]["items"] != null)
                                    {
                                        try
                                        {
                                            JToken jToken5 = val5["cues"]["items"];
                                            switch (jToken5.Count())
                                            {
                                                case 3:
                                                    PrevCue = jToken5[0]["t"]?.ToString();
                                                    CurrCue = jToken5[1]["t"]?.ToString();
                                                    NextCue = jToken5[2]["t"]?.ToString();
                                                    if (jToken5[0]["pgs"] != null && jToken5[0]["pgs"]["v"] != null)
                                                    {
                                                        CurrCueProgress = (float)jToken5[0]["pgs"]["v"];
                                                    }
                                                    if (jToken5[1]["pgs"] != null && jToken5[1]["pgs"]["v"] != null)
                                                    {
                                                        PrevCueProgress = (float)jToken5[1]["pgs"]["v"];
                                                    }
                                                    if (jToken5[2]["pgs"] != null && jToken5[2]["pgs"]["v"] != null)
                                                    {
                                                        NextCueProgress = (float)jToken5[2]["pgs"]["v"];
                                                    }
                                                    if (Executor.PrevCue != PrevCue || Executor.CurrCue != CurrCue || Executor.NextCue != NextCue || Executor.PrevCueProgress != CurrCueProgress || Executor.CurrCueProgress != PrevCueProgress || Executor.NextCueProgress != NextCueProgress)
                                                    {
                                                        IsChanged = true;
                                                    }
                                                    Executor.PrevCue = PrevCue;
                                                    Executor.CurrCue = CurrCue;
                                                    Executor.NextCue = NextCue;

                                                    Executor.PrevCueProgress = jToken5[0]["pgs"] != null && jToken5[0]["pgs"]["v"] != null ? CurrCueProgress : -1f;
                                                    Executor.CurrCueProgress = jToken5[1]["pgs"] != null && jToken5[1]["pgs"]["v"] != null ? PrevCueProgress : -1f;
                                                    Executor.NextCueProgress = jToken5[2]["pgs"] != null && jToken5[2]["pgs"]["v"] != null ? NextCueProgress : -1f;
                                                    break;
                                                case 1:
                                                    PrevCue = null;
                                                    CurrCue = jToken5[0]["t"]?.ToString();
                                                    NextCue = null;
                                                    if (jToken5[0]["pgs"] != null && jToken5[0]["pgs"]["v"] != null)
                                                    {
                                                        PrevCueProgress = (float)jToken5[0]["pgs"]["v"];
                                                    }
                                                    if (Executor.PrevCue != PrevCue || Executor.CurrCue != CurrCue || Executor.NextCue != NextCue || Executor.CurrCueProgress != PrevCueProgress)
                                                    {
                                                        IsChanged = true;
                                                    }
                                                    Executor.PrevCue = PrevCue;
                                                    Executor.CurrCue = CurrCue;
                                                    Executor.NextCue = NextCue;
                                                    if (jToken5[0]["pgs"] != null && jToken5[0]["pgs"]["v"] != null)
                                                    {
                                                        Executor.CurrCueProgress = PrevCueProgress;
                                                    }
                                                    else
                                                    {
                                                        Executor.CurrCueProgress = -1f;
                                                    }
                                                    Executor.PrevCueProgress = -1f;
                                                    Executor.NextCueProgress = -1f;
                                                    break;
                                                default:
                                                    PrevCue = null;
                                                    CurrCue = null;
                                                    NextCue = null;
                                                    if (Executor.PrevCue != PrevCue || Executor.CurrCue != CurrCue || Executor.NextCue != NextCue || Executor.CurrCueProgress != PrevCueProgress)
                                                    {
                                                        IsChanged = true;
                                                    }
                                                    Executor.PrevCue = PrevCue;
                                                    Executor.CurrCue = CurrCue;
                                                    Executor.NextCue = NextCue;
                                                    Executor.CurrCueProgress = -1f;
                                                    break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message + "!5");
                                        }
                                    }
                                    else
                                    {
                                        Executor.PrevCue = null;
                                        Executor.CurrCue = null;
                                        Executor.NextCue = null;
                                        Executor.CurrCueProgress = -1f;
                                    }

                                    //if (IsChanged)
                                    //{
                                    //    ExecutorChanged(this, new MsgFeedbackMA2Executor
                                    //    {
                                    //        ExecutorPage = buttonsPageNumber + 1,
                                    //        ExecutorNumber = num11 + n + 1,
                                    //        IsButton = true,
                                    //        LEDOn = (IsAssigned && State == ExecutorState.On),
                                    //        Executor = Executor
                                    //    });
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            lock (_Lock)
            {
                Timer.Stop();
            }
        }

        internal void ClearAll()
        {
            ExecutorData.Clear();
        }

        public void Update()
        {
            lock (_Lock)
            {
                if (!Timer.IsRunning || Timer.ElapsedMilliseconds >= 1000L)
                {
                    string FaderRequest = "{\"requestType\":\"playbacks\",\"startIndex\":[0],\"itemsCount\":[90],";
                    FaderRequest = FaderRequest + "\"pageIndex\":" + (FadersPageNumber - 1) + ",";
                    FaderRequest += "\"itemsType\":[2],\"view\":2,\"execButtonViewMode\":1,\"buttonsViewMode\":0,";
                    FaderRequest = FaderRequest + "\"session\":" + _Connector.SessionNumber + ", \"maxRequests\":1}";
                    try
                    {
                        if (_Connector.IsConnected)
                        {
                            _Connector.webSocket.Send(FaderRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "!7");
                    }


                    string ButtonRequest = "{\"requestType\":\"playbacks\",\"startIndex\":[100],\"itemsCount\":[90],";
                    ButtonRequest = ButtonRequest + "\"pageIndex\":" + (ButtonsPageNumber - 1) + ",";
                    ButtonRequest += "\"itemsType\":[3],\"view\":3,\"execButtonViewMode\":2,\"buttonsViewMode\":0,";
                    ButtonRequest = ButtonRequest + "\"session\":" + _Connector.SessionNumber + ", \"maxRequests\":1}";
                    try
                    {
                        if (_Connector.IsConnected)
                        {
                            _Connector.webSocket.Send(ButtonRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "!8");
                    }
                    Timer.Restart();
                }
            }
        }

        public void SetFaderLevel(int executorNumber, float valueNorm, int fadersPageNumber = -1)
        {
            if (fadersPageNumber <= 0)
            {
                fadersPageNumber = FadersPageNumber;
            }
            executorData.Push(new ExecutorData
            {
                PageNo = fadersPageNumber,
                ExecNo = executorNumber,
                Value = valueNorm
            });
        }

        private void ApplyExecutorData(int ExecNo, float FaderVal, float PageNo)
        {
            string text = "{\"requestType\":\"playbacks_userInput\",";
            text = text + "\"execIndex\":" + (ExecNo - 1) + ",";
            text = text + "\"pageIndex\":" + (PageNo - 1f) + ",";
            text = text + "\"faderValue\":" + FaderVal.ToString(CultureInfo.InvariantCulture) + ",";
            text += "\"type\":1,";
            text = text + "\"session\":" + _Connector.SessionNumber + ",";
            text += "\"maxRequests\":0}";
            try
            {
                if (_Connector.IsConnected)
                {
                    _Connector.webSocket.Send(text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "!9");
            }
        }

        public void SetButtonState(int executorNumber, int buttonNumber, bool pressed, int pageNumber = -1)
        {
            if (pageNumber <= 0)
            {
                pageNumber = ButtonsPageNumber;
                if (executorNumber < 100)
                {
                    pageNumber = FadersPageNumber;
                }
            }
            string str = "{\"requestType\":\"playbacks_userInput\",";
            str += "\"cmdline\":\"\",";
            str = str + "\"execIndex\":" + (executorNumber - 1) + ",";
            str = str + "\"pageIndex\":" + (pageNumber - 1) + ",";
            str = str + "\"buttonId\":" + (buttonNumber - 1) + ",";
            str = str + "\"pressed\":" + pressed.ToString() + ",";
            str = str + "\"released\":" + (!pressed).ToString() + ",";
            str += "\"type\":0,";
            str = str + "\"session\":" + _Connector.SessionNumber + ",";
            str += "\"maxRequests\":0}";
            try
            {
                if (_Connector.IsConnected)
                {
                    _Connector.webSocket.Send(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "!10");
            }
        }
    }
}
