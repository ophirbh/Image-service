﻿using ImageService.Communication.Client;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// Represents a log model.
    /// </summary>
    public class LogsModel
    {
        private bool canAcceptLogs = false;
        public IClientCommunicationChannel Client { get; set; }

        /// <summary>
        /// Gets a client Instance and registers to the ServerCommandRecieved event.
        /// </summary>
        public LogsModel()
        {
            Client = TcpClientChannel.Instance;
            Client.ServerCommandRecieved += ServerCommandRecieved;
            Initializer();

        }

        /// <summary>
        /// Initializes a new log model.
        /// Sends the Server a LogCommand to recieve all the logs so far.
        /// </summary>
        private void Initializer()
        {
            Logs = new List<MessageRecievedEventArgs>();
            CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, null, "");
            Client.SendCommand(request);
        }

        /// <summary>
        /// Recieves a new command.
        /// </summary>
        /// <param name="commandRead">The commnad received arguments.</param>
        private void ServerCommandRecieved(CommandRecievedEventArgs commandRead)
        {
            //LogCommand recieved. We add the Logs to the Logs ObservableCollection.
            if (commandRead != null && commandRead.CommandID == (int)CommandEnum.LogCommand)
            {
                Object thisLock = new Object();
                //BindingOperations.EnableCollectionSynchronization(Logs, thisLock);
                List<MessageRecievedEventArgs> recievedLogs = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(commandRead.Args[0]);
                foreach (MessageRecievedEventArgs log in recievedLogs)
                {
                    //App.Current.Dispatcher.Invoke((Action)delegate
                    //{
                    Logs.Add(log);
                    //});
                }
                canAcceptLogs = true;
            }
            //NewLogCommand recieved. We add the Log to the Logs ObservableCollection.
            else if (commandRead != null && commandRead.CommandID == (int)CommandEnum.NewLogCommand && Logs != null && canAcceptLogs == true)
            {
                Object thisLock = new Object();
                //BindingOperations.EnableCollectionSynchronization(Logs, thisLock);
                MessageRecievedEventArgs recievedLog = JsonConvert.DeserializeObject<MessageRecievedEventArgs>(commandRead.Args[0]);
                //App.Current.Dispatcher.Invoke((Action)delegate
                // {
                Logs.Add(recievedLog);
                //});
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public List<MessageRecievedEventArgs> Logs { get; set; }
    }
}