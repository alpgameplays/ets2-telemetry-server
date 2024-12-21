using System;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public interface ISerialPortManager
    {
        event EventHandler<SerialPortChangedEventArgs> PortChanged;
        void OpenPort(string portName);
        void ClosePort();
        bool IsPortOpen();
        void WriteToPort(string message);
    }
}
