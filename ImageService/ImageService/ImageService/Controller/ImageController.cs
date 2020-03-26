using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// Represents an image controller.
    /// </summary>
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private ILoggingService m_logging;
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// Constructor. Creates a new image controller.
        /// </summary>
        /// <param name="modal">The image service modal.</param> 
        public ImageController(IImageServiceModal modal, ILoggingService logging)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            m_logging = logging;
            commands = new Dictionary<int, ICommand>();

            commands[(int)CommandEnum.NewFileCommand] = new NewFileCommand(m_modal);
            commands[(int)CommandEnum.GetConfigCommand] = new GetConfigCommand();
            commands[(int)CommandEnum.LogCommand] = new LogCommand(m_logging);
        }

        /// <summary>
        /// Executes the given command in his own task.
        /// </summary>
        /// <param name="commandID">The command's identification.</param> 
        /// <param name="args">The command's arguments.</param> 
        /// <param name="resultSuccesful">The command's result.</param> 
        /// <returns>A string with information about the success/failure of the assignment.</returns> 
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            Task <Tuple<string, bool>> command = new Task <Tuple<string, bool>>(() =>
              {
                  bool resultSuccesfulCheck;
                  string msg= commands[commandID].Execute(args, out resultSuccesfulCheck);
                  return Tuple.Create(msg, resultSuccesfulCheck);
              });
            command.Start();
            command.Wait();
            Tuple<string, bool> result = command.Result;
            resultSuccesful = result.Item2;
            return result.Item1;

        }
    }
}
