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
    /// Represents a main window model.
    /// </summary>
    class MainWindowModel : IMainWindowModel
    {
        public IClientCommunicationChannel Client { get; set; }

        /// <summary>
        /// Gets a client Instance and recieved the IsConnected status.
        /// </summary>
        public MainWindowModel()
        {
            Client = TcpClientChannel.Instance;
            Connected = Client.IsConnected;
        }

        private bool connected;
        public bool Connected
        {
            get { return connected; }
            set
            {
                connected = value;
                NotifyPropertyChanged("Connected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies that a property has changed.
        /// </summary>
        /// <param name="propName">The property name.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
