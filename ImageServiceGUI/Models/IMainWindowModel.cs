using ImageService.Communication.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    /// <summary>
    /// Represents a main window model Interface.
    /// </summary>
    interface IMainWindowModel : INotifyPropertyChanged
    {
        IClientCommunicationChannel Client { get; set; }
        bool Connected {get; set;}
    }
}
