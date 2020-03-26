using ImageService.Communication.Client;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;


namespace ImageServiceWeb.Models
{
    /// <summary>
    /// Represents a config model.
    /// </summary>
    public class ConfigModel
    {
        public IClientCommunicationChannel Client { get; set; }

        /// <summary>
        /// Gets a client Instance and registers to the ServerCommandRecieved event.
        /// </summary>
        public ConfigModel()
        {
            Client = TcpClientChannel.Instance;
            Client.RecieveCommand();
            Client.ServerCommandRecieved += ServerCommandRecieved;
            Initializer();
        }

        /// <summary>
        /// Initializes a new config model.
        /// Sends the Server a GetConfigCommand.
        /// </summary>
        private void Initializer()
        {
            Handlers = new List<string>();
            string[] arr = new string[1];
            CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arr, "");
            Client.SendCommand(request);
        }

        /// <summary>
        /// Recieves command from server.
        /// </summary>
        /// <param name="commandRead">The command recieved arguments.</param>
        private void ServerCommandRecieved(CommandRecievedEventArgs commandRead)
        {
            //GetConfigCommand recieved. We set the correct properties and add the handlers to the Handlers ObservableCollection.
            if (commandRead != null && commandRead.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                OutputDir = commandRead.Args[0];
                SourceName = commandRead.Args[1];
                LogName = commandRead.Args[2];
                ThumbnailSize = commandRead.Args[3];
                Object thisLock = new Object();
                //BindingOperations.EnableCollectionSynchronization(Handlers, thisLock);
                if (commandRead.Args[4] != "")
                {
                    string[] handlers = commandRead.Args[4].Split(';');
                    foreach (string handler in handlers)
                    {
                        if (handler != "")
                        {
                            //App.Current.Dispatcher.Invoke((Action)delegate
                           // {
                                Handlers.Add(handler);
                            //});
                        }
                    }
                }
            }

            //CloseCommand recieved. We remove the given handler from the Handlers ObservableCollection.
            else if (commandRead != null && commandRead.CommandID == (int)CommandEnum.CloseCommand)
            {
                Object thisLock = new Object();
               // BindingOperations.EnableCollectionSynchronization(Handlers, thisLock);
                if (Handlers.Contains(commandRead.Args[0]))
                {
                    //App.Current.Dispatcher.Invoke((Action)delegate
                    //{
                        Handlers.Remove(commandRead.Args[0]);
                    //});
                }
            }
        }

        /// <summary>
        /// Sends the server CloseCommand to close the given handler.
        /// </summary>
        /// <param name="handler">handler to delete</param>
        public void DeleteHandler(string handler)
        {
            string[] arr = new string[1];
            arr[0] = handler;
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, arr, handler);
            Client.SendCommand(command);
        }

        //Properties.
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public string ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers")]
        public List<string> Handlers { get; set; }

    }
}