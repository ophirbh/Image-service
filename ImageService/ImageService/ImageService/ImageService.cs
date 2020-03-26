using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Configuration;
using ImageService.Infrastructure;
using ImageService.Communication.Server;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Threading;
using ImageService.ClientHandler;

namespace ImageService
{   
/// <summary>
    /// the ImageService class
    /// </summary>
    public partial class ImageService : ServiceBase
    {

        private ImageServer imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private IServerCommunicationChannel serverChannel;
        private IClientHandler clientHandler;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private int eventId = 1;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        /// <summary>
        /// getting the settings from the app config and Initializing the service
        /// </summary>
        /// <param name="args">arguments</param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            string handler = ConfigurationManager.AppSettings.Get("Handler");
            string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
            string thumbnailSize = ConfigurationManager.AppSettings.Get("ThumbnailSize");

            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;

            logging = new LoggingService();
            logging.MessageRecieved += OnMsg;
            modal = new ImageServiceModal(outputDir, int.Parse(thumbnailSize));
            controller = new ImageController(modal,logging);
            imageServer = new ImageServer(controller, logging, handler);
            //clientHandler = new HandleGuiClient(controller, logging, imageServer);
            clientHandler = new HandleAndroidClient(controller, logging, imageServer); 
            serverChannel = new TcpServerChannel(8000, logging, clientHandler);
            clientHandler.NotifyClients += serverChannel.NotifyClients;
        }

        /// <summary>
        /// the function starts when the service starts
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            logging.Log("In OnStart",MessageTypeEnum.INFO);
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            serverChannel.Start();
        }

        /// <summary>
        /// the function starts when the service stops, tells the server to close
        /// </summary>
        protected override void OnStop()
        {
            logging.Log("In onStop", MessageTypeEnum.INFO);
            System.Threading.Thread.Sleep(1000);
            imageServer.CloseServer();
            logging.MessageRecieved -= OnMsg;
            serverChannel.Stop();  
        }

        /// <summary>
        /// monitoring the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            logging.Log("Monitoring the System", MessageTypeEnum.INFO);
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        protected override void OnContinue()
        {
            logging.Log("In OnContinue.", MessageTypeEnum.INFO);
        }

        /// <summary>
        /// writes the messages received to the log
        /// </summary>
        /// <param name="sender">the sender </param>
        /// <param name="e">the arguments</param>
        private void OnMsg(object sender, MessageRecievedEventArgs e)
        {
            eventLog1.WriteEntry(e.Message, GetType(e.Status));
        }

        /// <summary>
        /// getting the type according to the status
        /// </summary>
        /// <param name="status"></param>
        /// <returns>the type</returns>
        private EventLogEntryType GetType(MessageTypeEnum status)
        {
            switch (status)
            {
                case MessageTypeEnum.FAIL:
                    return EventLogEntryType.Error;
                case MessageTypeEnum.WARNING:
                    return EventLogEntryType.Warning;
                case MessageTypeEnum.INFO:
                default:
                    return EventLogEntryType.Information;
            }
        }


    }
}
