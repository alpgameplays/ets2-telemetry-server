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
        private readonly object _lock = new object();
        private Thread _monitorThread;

        public event EventHandler<SerialPortChangedEventArgs> PortChanged;

        public void OpenPort(string portName)
        {
            lock (_lock)
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

                var devicePortName= ArduinoHleper.GetDeviceName(_currentPortName);
                Program.NotifierMessage.LogMessage = new LogMessage(true, $"Conexão estabelecida com {devicePortName}\nDispositivos: {deviceName}");

                StartMonitoring();

            }
        }

        public void ClosePort()
        {
            lock (_lock)
            {
                _monitorThread?.Abort();
                _serialPort?.Close();
                _serialPort = null;
            }
        }

        public bool IsPortOpen()
        {
            return _serialPort?.IsOpen == true;
        }

        public void WriteToPort(string message)
        {
            _serialPort?.WriteLine(message);
        }

        private void StartMonitoring()
        {
            _monitorThread = new Thread(() =>
            {
                while (true)
                {
                    string newPortName = Settings.Instance.ArduinoPort;

                    if (newPortName != _currentPortName)
                    {
                        OnPortChanged(newPortName);
                        _currentPortName = newPortName;

                    }

                    Thread.Sleep(1000); 
                }
            });

            _monitorThread.Start();
        }

        protected virtual void OnPortChanged(string newPortName)
        {
            PortChanged?.Invoke(this, new SerialPortChangedEventArgs(_currentPortName, newPortName));

            OpenPort(newPortName);
        }
    }
}
