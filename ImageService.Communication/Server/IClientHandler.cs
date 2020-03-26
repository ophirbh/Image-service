using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Infrastructure;

namespace ImageService.Communication.Server
{
    /// <summary>
    /// Represents a client handler.
    /// </summary>
    public delegate void NotifyClients(CommandRecievedEventArgs command);
    public interface IClientHandler
    {
        event NotifyClients NotifyClients;
        void HandleClient(TcpClient client, List<TcpClient> a_clients);
        object getLock();
    }
}
