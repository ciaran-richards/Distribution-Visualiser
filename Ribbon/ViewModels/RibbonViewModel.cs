using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Creator.Views;
using Creator.Windows;
using DataSources.Windows;
using Microsoft.Win32;
using Prism.Events;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace Ribbon.ViewModels
{
    public class RibbonViewModel : BindableBase
    {

        private string _selectedItem;
        private BitmapImage _mainIcon;
        private IEventAggregator _ea;
        private string _message;
        private List<Button> _ribbonItems;
        private Window _creatorWindow;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        public List<Button> RibbonItems
        {
            get { return _ribbonItems; }
            set { SetProperty(ref _ribbonItems, value); }
        }

        public List<Button> GetButtons()
        {
            var newDist = new Button();
            newDist.Content = "New Distribution";
            newDist.Command = new DelegateCommand(NewDistributionWindow);
            newDist.FontSize = 15;

            var dataBase = new Button();
            dataBase.Content = "Create / Test Database";
            dataBase.Command = new DelegateCommand(NewDataBaseWindow);
            dataBase.FontSize = 15;

            var toggle = new Button();
            toggle.Content = ToggleText();
            toggle.Command = new DelegateCommand(SwitchDatabase);
            ToggleColour(toggle);
            toggle.FontSize = 15;

            var refresh = new Button();
            refresh.Content = "Refresh";
            refresh.Command = new DelegateCommand(Refresh());
            refresh.FontSize = 15;

            return new List<Button>()
            {
                newDist, dataBase,toggle, refresh
            };
        }
        public void SwitchDatabase()
        {
            var sqlService = new DistributionDataSerivice();
            sqlService.ToggleUseDatabase();
            RibbonItems = GetButtons();
        }

        public string ToggleText()
        {
            var sqlService = new DistributionDataSerivice();
            var useFlag = sqlService.UseDatabaseFlag();
            if (sqlService.TestConnection().IsFailure || useFlag.IsFailure)
                return "Database Offline";
            if (useFlag.Value) return "Save to: Database";
            return "Save to:    CSV     ";
        }

        public void ToggleColour(Button button)
        {
            var sqlService = new DistributionDataSerivice();
            var useFlag = sqlService.UseDatabaseFlag();
            if (sqlService.TestConnection().IsFailure || useFlag.IsFailure)
            {
                button.Background = Brushes.LightGray;
                return;
            } 
            if (!useFlag.Value)
                button.Background = Brushes.LightGray;
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }


        private void SetSmallProperties(Window window)
        { 
            window.Width = 480;
            window.Height = 360;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Icon = _mainIcon;
        }
        private void NewDistributionWindow()
        {
            _creatorWindow = new CreatorWindow();
            SetSmallProperties(_creatorWindow);
            _creatorWindow.Show();
        }

        private void NewDataBaseWindow()
        { 
            _creatorWindow = new DataSourceWindow();
           SetSmallProperties(_creatorWindow);
           _creatorWindow.ShowDialog();
        }

        private void CloseCreator()
        {
            if (_creatorWindow != null)
                _creatorWindow.Close();
        }

        private Action Refresh() => _ea.GetEvent<RefreshEvent>().Publish;

        public RibbonViewModel(IEventAggregator aggregator)
        {
            var directory = Environment.CurrentDirectory;
            _mainIcon = new BitmapImage(new Uri(Pathing.Icon));
            _ea = aggregator;
            _ea.GetEvent<DistributionAddedEvent>().Subscribe(row => CloseCreator());
            Message = "New Distribution";
            RibbonItems = GetButtons();
        }
    }
}
