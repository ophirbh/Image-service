using ImageService.Communication.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    /// <summary>
    /// Represents a settings window model Interface.
    /// </summary>
    interface ISettingsModel :INotifyPropertyChanged
    {
        //properties
        string OutputDir { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> Handlers { get; set; }
        IClientCommunicationChannel Client { get; set; } 
    }
}
