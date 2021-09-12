using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProbabilityGUI.APIs
{
  
        public class AppCloseCommand : ICommand
        {
            public bool CanExecute(object parameter)
            {
                return Application.Current != null && Application.Current.MainWindow != null;
            }

            public void Execute(object parameter)
            {
                var process = Process.GetCurrentProcess();
                process.Close();
            }

            public event EventHandler CanExecuteChanged;
        }
}
