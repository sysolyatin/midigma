using System.Collections.Generic;

namespace MidiToGrandMA.DomainModel
{
    class ProjectFile
    {
        public string MidiDeviceName { get; set; }
        public int MidiDeviceId { get; set; }
        public string GmaIPAddress { get; set; }
        public string GmaLogin { get; set; }
        public string GmaPassword { get; set; }
        public List<Command> Commands { get; set; }

        public void SetForEmpty()
        {
            MidiDeviceName = string.Empty;
            MidiDeviceId = 0;
            GmaIPAddress = string.Empty;
            GmaLogin = string.Empty;
            GmaPassword = string.Empty;
            Commands = new List<Command>();
        }
    }
}
