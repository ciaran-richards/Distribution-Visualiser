using Prism.Commands;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using CSharpFunctionalExtensions;
using Prism.Events;
using ProbabilitySolver;
using ProbabilitySolver.Structs;
using ProbabilitySolver.Services;
using ProbabilitySolver.Services.InputService;
using Shared.Events;

namespace Creator.ViewModels
{
    public class CreatorViewModel : BindableBase
    {

        private IEventAggregator _ea;

        private string _name;

        private string _infoOne; 
        
        private string _infoTwo;

        private string _inputOne;

        private string _inputTwo;

        private bool _createButtonEnabled;

        private Brush _validBrush = Brushes.LightGreen;
        private Brush _invalidBrush = Brushes.IndianRed;

        private Brush _inputOneBrush;
        private Brush _inputTwoBrush; 
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string InfoOne
        {
            get { return _infoOne; }
            set { SetProperty(ref _infoOne, value); }
        }
        public string InfoTwo
        {
            get { return _infoTwo; }
            set { SetProperty(ref _infoTwo, value); }
        }

        public string InputOne
        {
            get { return _inputOne; }
            set { SetProperty(ref _inputOne, value); }
        }

        public string InputTwo
        {
            get { return _inputTwo; }
            set { SetProperty(ref _inputTwo, value); }
        }

        public IInputService<uint, Small> inSmall { get; set; }
        public IInputService<double, Probability> inProb { get; set; }

        public IInputService<double, double> inDouble { get; set; }

        public Brush InputOneBrush
        {
            get { return _inputOneBrush; }
            set { SetProperty(ref _inputOneBrush, value); }
        }

        public Brush InputTwoBrush
        {
            get { return _inputTwoBrush; }
            set { SetProperty(ref _inputTwoBrush, value); }
        }

        public bool CreateButtonEnabled
        {
            get { return _createButtonEnabled; }
            set { SetProperty(ref _createButtonEnabled, value); }
        }

        public DelegateCommand TypeChangedCommand { get; private set; }

        public DelegateCommand InputOneChangedCommand { get; private set; }

        public DelegateCommand InputTwoChangedCommand { get; private set; }

        public DistributionEnums SelectedType { get; set; }

        public List<DistributionEnums> DistributionTypes { get { return new List<DistributionEnums>(){DistributionEnums.Binomial, DistributionEnums.Normal};} }

        public DelegateCommand CreateCommand { get; private set; }

        private string CreateInfo()
        {
            switch (SelectedType)
            {
                case (DistributionEnums.Binomial):
                { 
                    return  $"B[{InputOne},{InputTwo}]";
                }

                case (DistributionEnums.Normal):
                {
                    return $"N[{InputOne},{InputTwo}]";
                }
            }

            return "";
        }

        public void Create()
        {
            _ea.GetEvent<NewDistributionRequest>().Publish(new NewDistributionInfo(Name, InputOne, InputTwo, CreateInfo(), SelectedType));
        }


        public void Changed<TMid,TEnd>(string input, IInputService<TMid, TEnd> inputService, out Result<TEnd> result, out SolidColorBrush brush)
        {
            var run = inputService.Run(input);
            if (run.IsSuccess)
            {
                brush = (SolidColorBrush)_validBrush;
            }
            else
            {
                brush = (SolidColorBrush)_invalidBrush;
            }

            result = run;
        }

        public void TypeChanged()
        {
            switch (SelectedType)
            {
                case (DistributionEnums.Binomial):
                {
                    InfoOne = "Cases:";
                    InfoTwo = "Probability:";
                    break;
                }

                case (DistributionEnums.Normal):
                {
                    InfoOne = "Mean:";
                    InfoTwo = "Standard Deviation:";
                    break;
                }
            }
            UpdateCreateButton();
        }

        public void UpdateCreateButton()
        {
            CreateButtonEnabled = CreateEnabled(SelectedType, InfoOne, InfoTwo);
        }

        public bool CreateEnabled(DistributionEnums distribution, string infoOne, string infoTwo)
        {
            switch (distribution)
            {
                case (DistributionEnums.Binomial):
                {

                    bool oneValid = inSmall.Run(InputOne).IsSuccess;
                    bool twoValid = inProb.Run(InputTwo).IsSuccess;
                    return (oneValid && twoValid);
                }

                case (DistributionEnums.Normal):
                {
                    try 
                    {
                        var tryParse = double.Parse(InputOne);
                        tryParse = double.Parse(InputTwo);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return true;
                }

            }

            return false;
        }



        public CreatorViewModel(IEventAggregator ea)
        {
            _ea = ea;
            Name = "Distribution Name";
            SelectedType = DistributionEnums.Binomial;
            InfoOne = "InfoOne";
            InfoTwo = "InfoTwo";
            inSmall = new InputServiceSmallInt();
            inProb = new InputServiceProbability();
            inDouble = new InputServiceDouble();
            InputOneBrush = new SolidColorBrush(Colors.LightBlue);
            InputTwoBrush = new SolidColorBrush(Colors.LightBlue);
            var smallResult = new Result<Small>();
            var decimalResult = new Result<Probability>();
            var doubleResult = new Result<double>();
            var colorBrush = Brushes.LightGreen;

            TypeChangedCommand = new DelegateCommand(TypeChanged);
            InputOneChangedCommand = new DelegateCommand(() =>
            {
                switch (SelectedType)
                {
                    case (DistributionEnums.Binomial):
                        Changed(InputOne, inSmall, out smallResult, out colorBrush);
                        if (smallResult.IsFailure)
                            InfoOne = smallResult.Error;
                        else
                            TypeChanged();
                        break;
                    case (DistributionEnums.Normal):
                        Changed(InputOne, inDouble, out doubleResult, out colorBrush );
                       
                        if (doubleResult.IsFailure)
                            InfoOne = doubleResult.Error;
                        else
                            TypeChanged();
                        break;
                }
                UpdateCreateButton();
                InputOneBrush = colorBrush;
            });

            InputTwoChangedCommand = new DelegateCommand(() =>
            {
                switch (SelectedType)
                {
                    case (DistributionEnums.Binomial):
                        Changed(InputTwo, inProb, out decimalResult, out colorBrush);
                        if (decimalResult.IsFailure)
                            InfoTwo = decimalResult.Error;
                        else
                            TypeChanged();
                        break;
                    case (DistributionEnums.Normal):
                        Changed(InputTwo, inDouble, out doubleResult, out colorBrush);
                        if (doubleResult.IsFailure)
                            InfoTwo = doubleResult.Error;
                        else
                            TypeChanged(); 
                        break;
                }
                UpdateCreateButton();
                InputTwoBrush = colorBrush;
            });

            TypeChanged();

            CreateCommand = new DelegateCommand(Create);
        }
    }
}
