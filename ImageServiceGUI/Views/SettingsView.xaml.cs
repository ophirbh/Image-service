using ImageServiceGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageServiceGUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// Creates a SettingsViewModel and sets it to be the DataContext.
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SettingsViewModel settingsViewModel;

        public SettingsView()
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel();
            DataContext = settingsViewModel;
        }
    }
}
