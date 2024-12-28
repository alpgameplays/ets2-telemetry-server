using Funbit.Ets.Telemetry.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funbit.Ets.Telemetry.Server.Helpers
{
    public class MessageNotifier
    {

        private static LogMessage _logConnectionMessage = new LogMessage(false, "Initial message");
        public event Action<LogMessage> MessageChanged;

        public LogMessage LogMessage
        {
            get => _logConnectionMessage;
            set
            {
                if (_logConnectionMessage != value)
                {
                    _logConnectionMessage = value;
                    MessageChanged?.Invoke(_logConnectionMessage);
                }
            }
        }
    }
}
