using ImageService.Logging.Modal;
using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// Represents a logs view model.
    /// </summary>
    class LogsViewModel : INotifyPropertyChanged
    {
        private ILogModel logModel;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new logs view model instance.
        /// </summary>
        public LogsViewModel()
        {
            logModel = new LogModel();
            logModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        /// <summary>
        /// Notifies that a property has changed.
        /// </summary>
        /// <param name="propName">The property name.</param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ObservableCollection<MessageRecievedEventArgs> VM_Logs
        {
            get { return logModel.Logs; }
        }
    }
}
