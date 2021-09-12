using System;
using System.Diagnostics;
using System.Dynamic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using ProbabilityGUI.APIs;
using Shared.Events;

namespace ProbabilityGUI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IEventAggregator _ea;
        private string _title = "Probability Charts";

        public DelegateCommand ShutDown { get; private set; }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IEventAggregator ea)
        {
            ShutDown = new DelegateCommand(() =>
            {
               Environment.Exit(0);
            });
            _ea = ea;
            var PAPI = new ProbabilityAPI(ea);
        }

    }
}
