
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Funbit.Ets.Telemetry.Server.Helpers
{


    public class ArduinoHleper
    {

        public static ICollection<string> ListArduinoPorts()
        {
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                Console.WriteLine("Nenhuma porta COM encontrada!");

            }

            foreach (var port in ports)
            {
                Console.WriteLine($"- {port}");
            }

            return ports;

        }
        public static string GetDeviceName(string port)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%({port})%'"))
                {
                    foreach (var device in searcher.Get()) { 
                        return device["Caption"].ToString(); 
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Erro ao obter o nome do dispositivo para a porta {port}: {ex.Message}"); }
            return "Desconhecido";
        }
    }

}
