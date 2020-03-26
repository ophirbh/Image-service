using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Client
{
    public delegate void CommandRecievedFromServer(CommandRecievedEventArgs commandRead);

    /// <summary>
    /// Represents a client communication channel Interface.
    /// </summary>
    public interface IClientCommunicationChannel
    {
        event CommandRecievedFromServer ServerCommandRecieved;
        void SendCommand(CommandRecievedEventArgs commandRecievedEventArgs);
        void RecieveCommand();
        void CloseClient();
        bool IsConnected { get; }
    }
}
