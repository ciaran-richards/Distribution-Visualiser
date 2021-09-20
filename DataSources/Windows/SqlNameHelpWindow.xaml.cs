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
using System.Windows.Shapes;
using ProbabilitySolver.Structs;

namespace DataSources.Windows
{
    /// <summary>
    /// Interaction logic for SqlNameHelp.xaml
    /// </summary>
    public partial class SqlNameHelpWindow : Window
    {
        public SqlNameHelpWindow()
        {
            InitializeComponent();
            string directory = Environment.CurrentDirectory;
            Icon = new BitmapImage(new Uri(Pathing.Icon));
            Helpimage.Source = new BitmapImage(new Uri(Pathing.ServerNameHelp));
        }
    }
}
