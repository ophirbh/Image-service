using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageServiceGUI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ImageServiceGUI.ViewModels
{
    /// <summary>
    /// Represents a settings view model.
    /// </summary>
    class SettingsViewModel :INotifyPropertyChanged
    {
        private ISettingsModel settingsModel;
        public ICommand RemoveCommand { get; private set; }
        
        /// <summary>
        /// Creates a settings view model instance.
        /// </summary>
        public SettingsViewModel()
        {
            settingsModel =new SettingsModel();
            settingsModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
              {
                  NotifyPropertyChanged("VM_" + e.PropertyName);
              };

            RemoveCommand = new DelegateCommand<object>(OnRemove, CanRemove);
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

        //Properties
        public string VM_OutputDir
        {
            get { return settingsModel.OutputDir; }
        }

        public string VM_SourceName
        {
            get { return settingsModel.SourceName; }
        }

        public string VM_LogName
        {
            get { return settingsModel.LogName; }
        }

        public string VM_ThumbnailSize
        {
            get { return settingsModel.ThumbnailSize; }
        }

        public ObservableCollection<string> VM_Handlers
        {
            get { return settingsModel.Handlers; }
        }

        private string selectedHandler;
        public string SelectedHandler
        {
            get { return selectedHandler; }
            set
            {
                selectedHandler = value;
                if(RemoveCommand as DelegateCommand<object> !=null)
                    (RemoveCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
            }
        }
        
        /// <summary>
        /// Checks if can be removed.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        private bool CanRemove(object obj)
        {
            if (SelectedHandler != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Initiates remove procedure.
        /// Sends the server CloseCommand.
        /// </summary>
        /// <param name="obj">The argument object.</param>
        private void OnRemove(object obj)
        {
            string[] arr = new string[1];
            arr[0] = selectedHandler;
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, arr, selectedHandler);
            settingsModel.Client.SendCommand(command);
        }
   
    }
}
