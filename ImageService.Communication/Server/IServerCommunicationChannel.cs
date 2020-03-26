using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Server
{
    /// <summary>
    /// Represents a server communication channel.
    /// </summary>
    public interface IServerCommunicationChannel 
    {
        void Start();
        void Stop();
        void NotifyClients(CommandRecievedEventArgs commandRecievedEventArgs);
    }
}
