using ImageService.Communication.Server;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Client
{
    public class TcpClientHandler :IClientHandler
    {
        public void HandleClient(TcpClient client,List<TcpClient>c)
        {
            new Task(() =>
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);

                    while (true)
                    {
                        Console.WriteLine("in Client handler");
                        string command = reader.ReadString();
                        //if (command == null)
                        // continue;
                        //m_logging.Log("HandleClient got the command " + command, MessageTypeEnum.INFO);
                        Console.WriteLine("read the command "+command);
                        CommandRecievedEventArgs commandRecievedEventArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(command);
                        if (commandRecievedEventArgs.CommandID == (int)CommandEnum.ClientClosedCommand)
                        {
                            //clients.Remove(client);
                            client.Close();
                            //m_logging.Log("A client was removed ", MessageTypeEnum.INFO);
                            break;
                        }
                        bool result;
                        //string commandStr = m_controller.ExecuteCommand(commandRecievedEventArgs.CommandID, commandRecievedEventArgs.Args, out result);
                        //mutexLock.WaitOne();
                        //writer.Write(commandStr);
                        //mutexLock.ReleaseMutex();
                    }
                }
                catch (Exception exc)
                {
                    //clients.Remove(client);
                    client.Close();
                    //m_logging.Log(exc.ToString(), MessageTypeEnum.FAIL);
                }
            }).Start();

        }
    }
}

