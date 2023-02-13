using System;

namespace MidiToGrandMA
{
    public class MidiResponse
    {
        public MidiResponseTypes ResponseType { get; set; }
        public string Chanel { get; }
        public string ControllerId { get; }
        public int ControllerValue { get; }
        public string Id { get; }
        public string Title { get; }

        public string RawEvent { get; set; }

        public int PercentControllerValue => (int)Math.Round((double)(ControllerValue * 100 / 127));

        public MidiResponse(string midiEvent)
        {
            if (midiEvent.Contains("ControlChange")) ResponseType = MidiResponseTypes.ControlChange;
            if (midiEvent.Contains("NoteOn")) ResponseType = MidiResponseTypes.NoteOn;
            if (midiEvent.Contains("NoteOff")) ResponseType = MidiResponseTypes.NoteOff;

            var partitionEvent = midiEvent.Split(' ');
            var isControl = ResponseType == MidiResponseTypes.ControlChange;

            Chanel = isControl
                ? partitionEvent[3]
                : $"{partitionEvent[3]} {partitionEvent[4]}";
            ControllerId = isControl ? partitionEvent[5] : string.Empty;
            ControllerValue = isControl ? int.Parse(partitionEvent[7]) : 0;
            var controllerIdForPrint = ControllerId == string.Empty ? string.Empty : $"-{ControllerId}";
            Id = (isControl ? "Control" : "Note") + $"-{Chanel.Replace(' ', '_')}{controllerIdForPrint}";
            RawEvent = midiEvent;
            Title = isControl ? $"Control {ControllerId} (CH {Chanel})" : $"Note {Chanel}";
        }

    }

    public enum MidiResponseTypes
    {
        ControlChange,
        NoteOn,
        NoteOff
    }
}
