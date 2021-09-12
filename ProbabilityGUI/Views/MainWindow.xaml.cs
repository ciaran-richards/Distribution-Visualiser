using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProbabilityGUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var directory = Environment.CurrentDirectory;
            Icon = new BitmapImage(new Uri(directory + @"\Icon.png"));
            //EventHandler = new EventHandler();
        }
    }
}
