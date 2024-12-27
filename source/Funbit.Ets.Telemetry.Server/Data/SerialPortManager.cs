using Funbit.Ets.Telemetry.Server.Data;
using Funbit.Ets.Telemetry.Server.Helpers;
using System;
using System.CodeDom;
using System.IO.Ports;
using System.Threading;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public class SerialPortManager : ISerialPortManager
    {
        private SerialPort _serialPort;
        private string _currentPortName;

        public void OpenPort(string portName)
        {
            
            if (_serialPort != null && _serialPort.IsOpen)
            {
                ClosePort();
            }

            _currentPortName = portName;
            _serialPort = new SerialPort(portName, 115200)
            {
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None,
                DtrEnable = true,
                WriteTimeout = 5000
            };
            _serialPort.Open();

            var deviceName = _serialPort.ReadLine();

            var devicePortName = ArduinoHleper.GetDeviceName(_currentPortName);
            Program.NotifierMessage.LogMessage = new LogMessage(true, $"Conexão estabelecida com {devicePortName}\nDispositivos: {deviceName}");
            
        }

        public void ClosePort()
        {
            _serialPort?.Close();
            _serialPort = null;
        }

        public bool IsPortOpen()
        {
            return _serialPort?.IsOpen == true;
        }

        public void WriteToPort(string message)
        {
            _serialPort?.WriteLine(message);
        }

        public virtual void PortChange()
        {

            string newPortName = Settings.Instance.ArduinoPort;

            if (newPortName != _currentPortName)
            {

                _currentPortName = newPortName;
                OpenPort(newPortName);
            }

        }
    }
}
