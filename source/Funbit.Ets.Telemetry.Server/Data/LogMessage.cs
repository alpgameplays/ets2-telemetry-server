using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funbit.Ets.Telemetry.Server.Data
{
    public class LogMessage
    {
        public LogMessage(bool isConnectionMessage, string message ) {
            IsConnectionMessage = isConnectionMessage;
            Message = message;
        }
        public bool IsConnectionMessage { get; set; }
        public string Message { get; set; }
    }
}
