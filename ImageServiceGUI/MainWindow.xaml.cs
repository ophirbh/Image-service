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

namespace ImageServiceGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Creates a MainWindowVM and sets it to be the DataContext.
    /// </summary>
    public partial class MainWindow : Window
    {   
        private MainWindowVM mainWindowVM ; 

        public MainWindow()
        {
            InitializeComponent();
            mainWindowVM = new MainWindowVM();
            DataContext = mainWindowVM;
        }
    }
}
