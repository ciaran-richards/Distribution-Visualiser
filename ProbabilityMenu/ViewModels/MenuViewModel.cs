using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Prism.Events;
using ProbabilitySolver.Services.FileService;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace ProbabilityMenu.ViewModels
{
    public class MenuViewModel : BindableBase
    {

        public IEnumerable<string> DistributionTypes
        {
            get { return new List<string>() { "Binomial", "Normal" }; }
        }

        private MenuButtonViewModel _selectedButton;

        private IEnumerable<MenuButtonViewModel> _itemCollection;

        private IEventAggregator _ea;

        public IEnumerable<MenuButtonViewModel> ItemCollection
        {
            get { return _itemCollection; }
            set { SetProperty(ref _itemCollection, value); }
        }

        public MenuButtonViewModel SelectedButton
        {
            get { return _selectedButton; }
            set { SetProperty(ref _selectedButton, value); }
        }

        public IDistributionQuery Csv;

        public IDistributionQuery Distribution;

        public DelegateCommand ChangeGraphCommand { get; private set; }

        public DelegateCommand DeleteCommand { get; private set; }

        public DelegateCommand SaveCsvCommand { get; private set; }

        private void ChangeGraph()
        {
            _ea.GetEvent<DistributionChangedEvent>().Publish(SelectedButton?.distributionData);
        }


        private void DeleteGraph()
        {
            var data = SelectedButton?.distributionData;
            _ea.GetEvent<DistributionDeleteRequest>().Publish(SelectedButton.distributionData);

            Refresh();
        }

        public void SaveCsc()
        {
            _ea.GetEvent<SaveCsvEvent>().Publish(SelectedButton.distributionData);
            Refresh();
        }

        public void Refresh()
        {
            ItemCollection = new List<MenuButtonViewModel>();
            //Parse both Data base and csv folder here
            var distMenu = new List<MenuButtonViewModel>();
            var distList = new List<DistributionRow>();

            var useFlagResult = new DistributionDataSerivice().UseDatabaseFlag();
            if (useFlagResult.IsSuccess && useFlagResult.Value)
            {
                {
                    var getDistributions = Distribution.GetAllDistributions();
                    if (getDistributions.IsSuccess)
                    {
                        distList.AddRange(getDistributions.Value);
                    }
                }
            }

            var data = Csv.GetAllDistributions();
            if (data.IsSuccess)
            {
                distList.AddRange(data.Value);
            }

            foreach (var distribution in distList)
            {
                distMenu.Add(new MenuButtonViewModel(distribution));
            }

            ItemCollection = distMenu;
        }



        public MenuViewModel(IEventAggregator aggregator)
        {
            Distribution = new DistributionDataSerivice();
            Csv = new CsvService();
            _ea = aggregator;
            _ea.GetEvent<DistributionAddedEvent>().Subscribe(row => Refresh());
            _ea.GetEvent<NewDataBaseEvent>().Subscribe(Refresh);
            _ea.GetEvent<RefreshEvent>().Subscribe(Refresh);
            ChangeGraphCommand = new DelegateCommand(ChangeGraph);
            DeleteCommand = new DelegateCommand(DeleteGraph);
            SaveCsvCommand = new DelegateCommand(SaveCsc);

            Refresh();
        }


    }

}
