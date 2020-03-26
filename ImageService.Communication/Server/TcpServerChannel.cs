using ImageService.Infrastructure;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication.Server
{
    /// <summary>
    /// Represents a TCP server communication channel.
    /// </summary>
    public class TcpServerChannel : IServerCommunicationChannel
    {
        private int port;
        private TcpListener tcpListener;
        private ILoggingService loggingService;
        private IClientHandler clientHandler;
        private List<TcpClient> tcpClients = new List<TcpClient>();
        private object writeLock;
        private object listLock = new object();

        /// <summary>
        /// Represents a TCP server channel.
        /// </summary>
        /// <param name="port">The server's port.</param>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="clientHandler">The client handler.</param>
        public TcpServerChannel(int port, ILoggingService loggingService, IClientHandler clientHandler)
        {
            this.port = port;
            this.clientHandler = clientHandler;
            this.loggingService = loggingService;
            writeLock = clientHandler.getLock();
        }

        /// <summary>
        /// Starts the server.
        /// Accepts new connections, adds them to the list of clients and activates the HandleClient function on each one.
        /// </summary>
        public void Start()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            tcpListener = new TcpListener(ipEndPoint);
            tcpListener.Start();
            loggingService.Log("Created a new TCP server channel.", MessageTypeEnum.INFO); 
            loggingService.Log("Waiting for connections...", MessageTypeEnum.INFO); 

            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        loggingService.Log("Established a new connection ",MessageTypeEnum.INFO);
                        lock (listLock)
                        {
                            tcpClients.Add(client);
                        }
                        clientHandler.HandleClient(client, tcpClients);
                    }
                    catch (SocketException se)
                    {
                       
                        loggingService.Log(se.ToString(), MessageTypeEnum.FAIL); 
                        break;
                    }
                }
                loggingService.Log("Server stopped", MessageTypeEnum.WARNING);
            });
            task.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            tcpListener.Stop();
        }

        /// <summary>
        /// Notifies all clients about a command recieved.
        /// </summary>
        /// <param name="commandRecievedEventArgs">The command's arguments.</param>
        public void NotifyClients(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            try
            {
                List<TcpClient> tcpClientsCopiedList = new List<TcpClient>(tcpClients);
                foreach (TcpClient tcpClient in tcpClientsCopiedList)
                {
                    new Task(() =>
                    {
                        try
                        {
                            NetworkStream stream = tcpClient.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                            lock (writeLock)
                            {
                                writer.Write(jsonCommand);
                            }
                        }
                        catch (Exception)
                        {            
                            tcpClients.Remove(tcpClient);
                            tcpClient.Close();
                        }
                    }).Start();
                }
            }
            catch (Exception exc)
            {
                loggingService.Log(exc.ToString(), MessageTypeEnum.FAIL);
            }
        }   

    }
}
