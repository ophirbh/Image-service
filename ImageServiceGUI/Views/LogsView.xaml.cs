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
    /// Interaction logic for LogsView.xaml
    /// Creates a LogsViewModel and sets it to be the DataContext.
    /// </summary>
    public partial class LogsView : UserControl
    {
        private LogsViewModel logsViewModel;

        public LogsView()
        {
            InitializeComponent();
            logsViewModel = new LogsViewModel();
            DataContext = logsViewModel;
        }
    }
}
