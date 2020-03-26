using ImageServiceGUI.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// Represents a main window view model.
    /// </summary>
    class MainWindowVM : INotifyPropertyChanged
    {
        private IMainWindowModel mainWindowModel;
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// Creates a new main window view model instance.
        /// </summary>
        public MainWindowVM()
        {
            mainWindowModel = new MainWindowModel();
            mainWindowModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            CloseCommand = new DelegateCommand<object>(OnClose, CanClose);
        }

        public bool VM_Connected
        {
            get { return mainWindowModel.Connected; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies that a property has changed.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Checks if can be closed.
        /// </summary>
        /// <param name="arg">The argument object.</param>
        /// <returns></returns>
        private bool CanClose(object arg)
        {
            return true;
        }

        /// <summary>
        /// Initiates the closing procedure.
        /// </summary>
        /// <param name="obj">The argument object.</param>
        private void OnClose(object obj)
        {
            mainWindowModel.Client.CloseClient();
        }
    }
}
