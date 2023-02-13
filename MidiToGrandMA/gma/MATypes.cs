using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MidiToGrandMA.gma
{
    public enum ExecutorState
    {
        Unassigned,
        On,
        Off
    }

    public class MA2Executor
    {
        public string Appearance { get; set; }
        public ExecutorState State { get; set; }
        public bool IsButton { get; set; }
        public bool IsEmpty { get; set; }
        public string Label { get; set; }
        public string FaderPriority { get; set; }
        public string Color { get; set; }
        public string Button3 { get; set; }
        public string Button2 { get; set; }
        public string Button1 { get; set; }
        public string CurrCue { get; set; }
        public string PrevCue { get; set; }
        public string NextCue { get; set; }
        public float FaderValue { get; set; }
        public float PrevCueProgress { get; set; }
        public float CurrCueProgress { get; set; }
        public float NextCueProgress { get; set; }


        public int PageNo { get; set; }
        public int ExecNo { get; set; }
        public int Width { get; internal set; }
    }

    public class PromptChangedEventArgs
    {
        public string lastPromptText;
        public string newPromptText;
    }

    public class CommandButtonLEDEventArgs
    {
        public string keyname;
        public bool value;
    }

    public class ExecutorChangedEventArgs
    {
        public MA2Executor exec;
    }
    public class CommandConfirmationEventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<string> Options { get; set; }
        public string DefaultOption { get; set; }
    }

    public class EncoderWheelState
    {
        public string attribute;
        public int resolution;
        public float scrollMult;

        public EncoderWheelState()
        {
            attribute = null;
            resolution = 0;
            scrollMult = 1f;
        }
    }

    internal class ExecutorData
    {
        public int PageNo;
        public int ExecNo;
        public float Value;
    }

    public enum MA2Hardkeys
    {
        [Description("None")]
        None = 0,
        [Description("Ch Pg +")]
        ChPg_Plus = 3,
        [Description("Ch Pg -")]
        ChPg_Minus = 4,
        [Description("Fd Pg +")]
        FdPg_Plus = 5,
        [Description("Fd Pg -")]
        FdPg_Minus = 6,
        [Description("Bt Pg +")]
        BtPg_Plus = 7,
        [Description("Bt Pg -")]
        BtPg_Minus = 8,
        [Description("Pause (Large)")]
        Pause_Large = 9,
        [Description("Go- (Large)")]
        Go_Minus_Large = 10,
        [Description("Go+ (Large)")]
        Go_Plus_Large = 11,
        [Description("X1")]
        X1 = 12,
        [Description("X2")]
        X2 = 13,
        [Description("X3")]
        X3 = 14,
        [Description("X4")]
        X4 = 0xF,
        [Description("X5")]
        X5 = 0x10,
        [Description("X6")]
        X6 = 17,
        [Description("X7")]
        X7 = 18,
        [Description("X8")]
        X8 = 19,
        [Description("X9")]
        X9 = 20,
        [Description("X10")]
        X10 = 21,
        [Description("X11")]
        X11 = 22,
        [Description("X12")]
        X12 = 23,
        [Description("X13")]
        X13 = 24,
        [Description("X14")]
        X14 = 25,
        [Description("X15")]
        X15 = 26,
        [Description("X16")]
        X16 = 27,
        [Description("X17")]
        X17 = 28,
        [Description("X18")]
        X18 = 29,
        [Description("X19")]
        X19 = 30,
        [Description("X20")]
        X20 = 0x1F,
        [Description("List")]
        List = 0x20,
        [Description("User 1")]
        User_1 = 33,
        [Description("User 2")]
        User_2 = 34,
        [Description("U1")]
        U1 = 36,
        [Description("U2")]
        U2 = 37,
        [Description("U3")]
        U3 = 38,
        [Description("U4")]
        U4 = 39,
        [Description("Nipple")]
        Nipple = 40,
        [Description("Fix")]
        Fix = 41,
        [Description("Select")]
        Select = 42,
        [Description("Off")]
        Off = 43,
        [Description("Temp")]
        Temp = 44,
        [Description("Top")]
        Top = 45,
        [Description("On")]
        On = 46,
        [Description("<<<")]
        Arrows_Left = 47,
        [Description("Learn")]
        Learn = 48,
        [Description(">>>")]
        Arrows_Right = 49,
        [Description("Go- (Small)")]
        Go_Minus_Small = 50,
        [Description("Pause (Small)")]
        Pause_Small = 51,
        [Description("Go+ (Small)")]
        Go_Plus_Small = 52,
        [Description("Oops")]
        Oops = 53,
        [Description("Esc")]
        Esc = 54,
        [Description("Edit")]
        Edit = 55,
        [Description("Goto")]
        Goto = 56,
        [Description("Update")]
        Update = 57,
        [Description("Time")]
        Time = 58,
        [Description("Store")]
        Store = 59,
        [Description("Blind")]
        Blind = 60,
        [Description("Freeze")]
        Freeze = 61,
        [Description("Preview")]
        Prvw = 62,
        [Description("Assign")]
        Assign = 0x3F,
        [Description("Align")]
        Align = 0x40,
        [Description("Black-Out")]
        Black_Out = 65,
        [Description("View")]
        View = 66,
        [Description("Effect")]
        Effect = 67,
        [Description("MA")]
        MA = 68,
        [Description("Delete")]
        Delete = 69,
        [Description("Page")]
        Page = 70,
        [Description("Macro")]
        Macro = 71,
        [Description("Preset")]
        Preset = 72,
        [Description("Copy")]
        Copy = 73,
        [Description("Sequ")]
        Sequence = 74,
        [Description("Cue")]
        Cue = 75,
        [Description("Exec")]
        Exec = 76,
        [Description("Channel")]
        Channel = 82,
        [Description("Fixture")]
        Fixture = 83,
        [Description("Group")]
        Group = 84,
        [Description("Move")]
        Move = 85,
        [Description("Num 0")]
        Num0 = 86,
        [Description("Num 1")]
        Num1 = 87,
        [Description("Num 2")]
        Num2 = 88,
        [Description("Num 3")]
        Num3 = 89,
        [Description("Num 4")]
        Num4 = 90,
        [Description("Num 5")]
        Num5 = 91,
        [Description("Num 6")]
        Num6 = 92,
        [Description("Num 7")]
        Num7 = 93,
        [Description("Num 8")]
        Num8 = 94,
        [Description("Num 9")]
        Num9 = 95,
        [Description("Plus")]
        Plus = 96,
        [Description("Minus")]
        Minus = 97,
        [Description("Dot")]
        Dot = 98,
        [Description("Full")]
        Full = 99,
        [Description("Highlight")]
        Highlight = 100,
        [Description("Solo")]
        Solo = 101,
        [Description("Thru")]
        Thru = 102,
        [Description("If")]
        If = 103,
        [Description("At")]
        At = 104,
        [Description("Clear")]
        Clear = 105,
        [Description("Please")]
        Please = 106,
        [Description("Up")]
        Up = 107,
        [Description("Set")]
        Set = 108,
        [Description("Previous")]
        Previous = 109,
        [Description("Next")]
        Next = 110,
        [Description("Down")]
        Down = 111,
        [Description("Help")]
        Help = 116,
        [Description("Backup")]
        Backup = 117,
        [Description("Setup")]
        Setup = 118,
        [Description("Tools")]
        Tools = 119,
        [Description("V1")]
        V1 = 120,
        [Description("V2")]
        V2 = 121,
        [Description("V3")]
        V3 = 122,
        [Description("V4")]
        V4 = 123,
        [Description("V5")]
        V5 = 124,
        [Description("V6")]
        V6 = 125,
        [Description("V7")]
        V7 = 126,
        [Description("V8")]
        V8 = 0x7F,
        [Description("V9")]
        V9 = 0x80,
        [Description("V10")]
        V10 = 129
    }

    public enum MA2KeyParam
    {
        [Description("0")]
        Num0,
        [Description("1")]
        Num1,
        [Description("2")]
        Num2,
        [Description("3")]
        Num3,
        [Description("4")]
        Num4,
        [Description("5")]
        Num5,
        [Description("6")]
        Num6,
        [Description("7")]
        Num7,
        [Description("8")]
        Num8,
        [Description("9")]
        Num9,
        [Description("PUNKT")]
        Dot,
        [Description("THRU")]
        Thru,
        [Description("PLUS")]
        Plus,
        [Description("MINUS")]
        Minus,
        [Description("IF")]
        If,
        [Description("AT")]
        At,
        [Description("OOPS")]
        Oops,
        [Description("ESC")]
        Esc,
        [Description("CLEAR")]
        Clear,
        [Description("FULL")]
        Full,
        [Description("SOLO")]
        Solo,
        [Description("HIGH")]
        High,
        [Description("ENTER")]
        Please,
        [Description("SET")]
        Set,
        [Description("PREV")]
        Prev,
        [Description("NEXT")]
        Next,
        [Description("ON")]
        On,
        [Description("OFF")]
        Off,
        [Description("ASSIGN")]
        Assign,
        [Description("COPY")]
        Copy,
        [Description("TIME")]
        Time,
        [Description("PAGE")]
        Page,
        [Description("MACRO")]
        Macro,
        [Description("DELETE")]
        Delete,
        [Description("GOTO")]
        GoTo,
        [Description("SELECT")]
        Select,
        [Description("SEQU")]
        Sequence,
        [Description("CUE")]
        Cue,
        [Description("PRESET")]
        Preset,
        [Description("ALIGN")]
        Align,
        [Description("EDIT")]
        Edit,
        [Description("UPDATE")]
        Update,
        [Description("EXEC")]
        Exec,
        [Description("STORE")]
        Store,
        [Description("GROUP")]
        Group,
        [Description("ODD")]
        Odd,
        [Description("EVEN")]
        Even,
        [Description("MA")]
        MA
    }

    public static class MA2Enums
    {
        public static string KeyEnumToCode(MA2KeyParam key)
        {
            try
            {
                return ((DescriptionAttribute)typeof(MA2KeyParam).GetMember(key.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]).Description;
            }
            catch
            {
            }
            return "";
        }
        public static string HardkeyToName(MA2Hardkeys hardkeycode)
        {
            try
            {
                return ((DescriptionAttribute)typeof(MA2Hardkeys).GetMember(hardkeycode.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]).Description;
            }
            catch
            {
            }
            return "";
        }
        public static int NameToHardkey(string name)
        {
            try
            {
                Type typeFromHandle = typeof(MA2Hardkeys);
                if (!typeFromHandle.IsEnum)
                {
                    throw new InvalidOperationException();
                }
                FieldInfo[] fields = typeFromHandle.GetFields();
                foreach (FieldInfo fieldInfo in fields)
                {
                    if (!(Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute))
                    {
                        if (fieldInfo.Name == name)
                        {
                            return (int)fieldInfo.GetValue(null);
                        }
                    }
                    else if (descriptionAttribute.Description == name)
                    {
                        return (int)fieldInfo.GetValue(null);
                    }
                }
            }
            catch
            {
            }
            return -1;
        }
        public static List<string> HardkeysList()
        {
            List<string> list = new List<string>();
            try
            {
                Type typeFromHandle = typeof(MA2Hardkeys);
                if (!typeFromHandle.IsEnum)
                {
                    throw new InvalidOperationException();
                }
                FieldInfo[] fields = typeFromHandle.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (Attribute.GetCustomAttribute(fields[i], typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
                    {
                        list.Add(descriptionAttribute.Description);
                    }
                }
                return list;
            }
            catch
            {
                return list;
            }
        }
    }

    internal class MA2ButtonQueue
    {
        public string Executor;
        public bool State;
    }

    internal class MaEncoderQueue : Queue<MA2ButtonQueue>
    {
        private readonly object FLock;

        public MaEncoderQueue()
        {
            FLock = new object();
        }

        public void Push(MA2ButtonQueue Data)
        {
            Enqueue(Data);
        }

        public MA2ButtonQueue Pull()
        {
            lock (FLock)
            {
                if (base.Count > 0)
                {
                    return Dequeue();
                }
                return null;
            }
        }
    }

    internal class ExecutorQueue : Queue<ExecutorData>
    {
        private readonly object FLock;

        public ExecutorQueue()
        {
            FLock = new object();
        }

        public void Push(ExecutorData ExecData)
        {
            lock (FLock)
            {
                try
                {
                    ExecutorData locData = this.FirstOrDefault(i => i.ExecNo == ExecData.ExecNo);
                    if (locData != null)
                    {
                        locData.Value = ExecData.Value;
                        return;
                    }
                }
                catch
                {
                }
            }
            Enqueue(ExecData);
        }

        public ExecutorData Pull()
        {
            lock (FLock)
            {
                if (base.Count > 0)
                {
                    return base.Dequeue();
                }
                return null;
            }
        }
    }

    [Serializable]
    public class MsgFeedbackMA2Executor
    {
        public MA2Executor Executor;
        public int ExecutorPage = 0;
        public int ExecutorNumber = 0;
        public bool IsButton { get; set; } = true;
        public bool LEDOn { get; set; }
        public string Appearance { get; set; } = "";
        //public System.Drawing.Color GetAppearanceColor()
        //{
        //    System.Drawing.Color result = System.Drawing.Color.Black;
        //    try
        //    {
        //        result = ColorTranslator.FromHtml(Appearance);
        //        return result;
        //    }
        //    catch
        //    {
        //        return result;
        //    }
        //}
    }

    public enum RemoteType
    {
        [Description("None")]
        None,
        [Description("Executor")]
        Exec,
        [Description("Command")]
        Cmd,
        [Description("Hard key")]
        Hardkey,
        [Description("Attribute")]
        Attribute,
        [Description("Encoder Wheel")]
        Wheel
    }

    public enum ButtonType
    {
        [Description("Button 3")]
        Button3 = 3,
        [Description("Button 2")]
        Button2 = 2,
        [Description("Fader")]
        Fader = 0,
        [Description("Button 1")]
        Button1 = 1
    }

 
    internal class MaDataQueue : Queue<ExecutorData>
    {
        private readonly object FLock;

        public MaDataQueue()
        {
            FLock = new object();
        }

        public void Push(ExecutorData ExecData)
        {
            lock (FLock)
            {
                try
                {
                    ExecutorData locData = this.FirstOrDefault(i => i.ExecNo == ExecData.ExecNo);
                    if (locData != null)
                    {
                        locData.Value = ExecData.Value;
                        return;
                    }
                }
                catch
                {
                }
            }
            Enqueue(ExecData);
        }

        public ExecutorData Dequeue2()
        {
            lock (FLock)
            {
                if (base.Count > 0)
                {
                    return Dequeue();
                }
                return null;
            }
        }
    }

}
