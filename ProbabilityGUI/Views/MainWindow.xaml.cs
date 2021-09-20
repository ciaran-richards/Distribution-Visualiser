using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;
using ProbabilitySolver.Structs;

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
            Icon = new BitmapImage(new Uri(Pathing.Icon));
            //EventHandler = new EventHandler();
        }
    }
}
