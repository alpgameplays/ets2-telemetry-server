using Funbit.Ets.Telemetry.Server.Data;
using Funbit.Ets.Telemetry.Server.Helpers;

using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public class ArduinoTelemetryController
    {
        private ISerialPortManager _serialPortManager;
        static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ArduinoTelemetryController(ISerialPortManager serialPortManager)
        {
            _serialPortManager = serialPortManager;
            _serialPortManager.PortChanged += OnPortChanged;
        }

        private void OnPortChanged(object sender, SerialPortChangedEventArgs e)
        {
            try
            {
                _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);
                Console.WriteLine($"Porta alterada de {e.OldPortName} para {e.NewPortName}");
                Program.NotifierMessage.LogMessage = new LogMessage(true, $"Porta alterada de {e.OldPortName} para {e.NewPortName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                Program.NotifierMessage.LogMessage = new LogMessage(true, $"Erro geral: {ex.Message}");
                Log.Error(ex);
                _serialPortManager.ClosePort();
                _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);
            }
        }

        public void RunApplication()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(50);
                        var json = Ets2TelemetryController.GetEts2TelemetryJson();

                        Console.WriteLine("Enviando JSON para o Arduino:");
                        Log.Info("Enviando JSON para o Arduino:");
                        Console.WriteLine(json);
                        Log.Info(json);

                        if (_serialPortManager.IsPortOpen())
                        {
                            _serialPortManager.WriteToPort(json);
                            Program.NotifierMessage.LogMessage = new LogMessage(false, $"JSON enviado com sucesso! {DateTime.Now}");
                            Console.WriteLine("JSON enviado com sucesso!");
                            Log.Info("JSON enviado com sucesso!");
                        }
                        else
                        {
                            _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);

                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Erro de operação inválida: {ex.Message}");
                        Program.NotifierMessage.LogMessage = new LogMessage(true, $"Erro de operação inválida: {ex.Message}");
                        Log.Error(ex);
                        _serialPortManager.ClosePort();
                        _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Erro de IO: {ex.Message}");
                        Program.NotifierMessage.LogMessage = new LogMessage(true, $"Erro de IO: {ex.Message}");
                        Log.Error(ex);
                        _serialPortManager.ClosePort();
                        _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro geral: {ex.Message}");
                        Program.NotifierMessage.LogMessage = new LogMessage(true, $"Erro geral: {ex.Message}");
                        Log.Error(ex);
                        _serialPortManager.ClosePort();
                        _serialPortManager.OpenPort(Settings.Instance.ArduinoPort);
                    }
                }
            });
        }
    }
}
