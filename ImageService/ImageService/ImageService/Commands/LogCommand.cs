using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Returns all the Logs from the start of the service.
    /// </summary>
    class LogCommand : ICommand
    {
        private ILoggingService m_logging;
        public LogCommand(ILoggingService logging)
        {
            m_logging = logging;      
        }

        /// <summary>
        /// Executes a log command.
        /// </summary>
        /// <param name="args">The command's arguments.</param>
        /// <param name="result">True if succeeded.</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                List<MessageRecievedEventArgs> logsList = m_logging.LogList;
                string jsonLogsList= JsonConvert.SerializeObject(logsList);
                string[] arr = new string[1];
                arr[0] = jsonLogsList;
                CommandRecievedEventArgs commandSend = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, arr, "");
                result = true;
                return JsonConvert.SerializeObject(commandSend);
            }
            catch (Exception exc)
            {
                result = false;
                return exc.ToString();
            }
        }
    }
}
