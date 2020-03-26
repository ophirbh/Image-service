using ImageService.Communication.Server;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Server
{
    /// <summary>
    /// Represents an image server.
    /// </summary>
    public class ImageServer 
    {    
        private IImageController m_controller;
        private ILoggingService m_logging;
        private string m_handler;
        public List<string> Handlers;
        
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; // The event that notifies about a new Command being recieved

        /// <summary>
        /// Constructor. creates a new image server instance.
        /// </summary>
        /// <param name="imageController">The image controller.</param> 
        /// <param name="loggingService">The logging service.</param> 
        /// <param name="handler">The paths to be handled separated by semicolons.</param>  
        public ImageServer(IImageController imageController, ILoggingService loggingService, string handler)
        {
            m_controller = imageController;
            m_logging = loggingService;
            m_handler = handler;

            Handlers = new List<string>();

            string[] paths = m_handler.Split(';');
            foreach (var path in paths)
            {
                Handlers.Add(path);
                CreateDirectoryHandler(path);
            }
        }

        /// <summary>
        /// Creates a new directory handler for the directory of the given path.
        /// </summary>
        /// <param name="path">The path of the directory to be handled.</param> 
        private void CreateDirectoryHandler(string path)
        {
            IDirectoryHandler directoryHandler = new DirectoyHandler(m_controller, m_logging);
            m_logging.Log("A directory handler was created for the directory in path: " + path, MessageTypeEnum.INFO);
            CommandRecieved += directoryHandler.OnCommandRecieved;
            directoryHandler.DirectoryClose += OnDirectoryClose;
            directoryHandler.StartHandleDirectory(path);
        }
        
        /// <summary>
        /// This method is called when a directory is closing.
        /// </summary>
        /// <param name="sender">The command sender.</param> 
        /// <param name="args">The directory close event arguments.</param> 
        private void OnDirectoryClose(object sender, DirectoryCloseEventArgs args)
        {
            if (sender is IDirectoryHandler)
            {
                IDirectoryHandler directoryHandler = (IDirectoryHandler)sender;
                CommandRecieved -= directoryHandler.OnCommandRecieved;
                m_logging.Log(args.Message, MessageTypeEnum.INFO);
            }
        }

        /// <summary>
        /// Close the server.invoke the command of it closing.
        /// </summary>
        public void CloseServer()
        {
            string[] args = { };
            CommandRecievedEventArgs e = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, "*");
            CommandRecieved?.Invoke(this, e);
            m_logging.Log("Server is Closing ", MessageTypeEnum.INFO);
        }

        public void makeEvent(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            CommandRecieved?.Invoke(this, commandRecievedEventArgs);
        }
    }
}

