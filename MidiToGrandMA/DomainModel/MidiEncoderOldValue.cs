using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiToGrandMA.DomainModel
{
    class MidiEncoderOldValue
    {
        public string MidiId { get; }
        public int MidiValue { get; private set; }

        public MidiEncoderOldValue(string midiId, int value)
        {
            MidiId = midiId;
            MidiValue = value;
        }
        public void UpdateValue(int value)
        {
            MidiValue = value;
        }

        public EncoderValue GetEncoderDirection(int midiValue)
        {
            var oldMidiValue = MidiValue;
            MidiValue = midiValue;
            if (midiValue == 127 || midiValue > oldMidiValue) return EncoderValue.Forward;
            if (midiValue == 0 || midiValue < oldMidiValue) return EncoderValue.Back;
            return EncoderValue.Stop;
        }
    }
}
