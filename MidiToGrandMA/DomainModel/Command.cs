using MidiToGrandMA.gma;

namespace MidiToGrandMA.DomainModel

{
    class Command
    {
        public CmdTypes CmdType { get; set; }
        public string MidiId { get; set; }
        public int? Executor { get; set; }
        public int? PageNumber { get; set; }
        public MA2Hardkeys? Hardkey { get; set; }
        public string Content { get; set; }


        public Command() { }

        public Command(CmdTypes cmdType, string midiId, int? executor, int? pageNumber, MA2Hardkeys? hardkey, string content)
        {
            CmdType = cmdType;
            MidiId = midiId;
            Content = content;
            Executor = executor;
            PageNumber = pageNumber;
            Hardkey = hardkey;
        }

        public bool IsExecutorControl()
        {
            return CmdType == CmdTypes.ExecutorButton1
                    || CmdType == CmdTypes.ExecutorButton2
                    || CmdType == CmdTypes.ExecutorButton3
                    || CmdType == CmdTypes.ExecutorFader;
        }
    }

    public enum CmdTypes
    {
        ExecutorFader,
        ExecutorButton1,
        ExecutorButton2,
        ExecutorButton3,
        Encoder,
        Hardkey,
        Command,
        SelectPresetType
    }
}