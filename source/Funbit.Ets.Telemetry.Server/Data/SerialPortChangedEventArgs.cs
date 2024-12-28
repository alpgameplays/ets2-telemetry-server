using System;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public class SerialPortChangedEventArgs : EventArgs
    {
        public string OldPortName { get; }
        public string NewPortName { get; }

        public SerialPortChangedEventArgs(string oldPortName, string newPortName)
        {
            OldPortName = oldPortName;
            NewPortName = newPortName;
        }
    }
}
