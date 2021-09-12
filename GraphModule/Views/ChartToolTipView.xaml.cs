using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace GraphModule.Views
{
    /// <summary>
    /// Interaction logic for ChartToolTip.xaml
    /// </summary>
    public partial class ChartToolTipView : IChartTooltip
    {
        private TooltipData data;
        private TooltipData xdata;
        public ChartToolTipView()
        {
            InitializeComponent();
            DataContext = this;
            SelectionMode = TooltipSelectionMode.SharedXValues;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public TooltipData Data
        {
            get => data;
            set
            {
                data = value;
                xData = new TooltipData();
                OnPropertyChanged("Data");       
            }
        }
        public TooltipData xData
        {
            get => xdata;
            set
            {
                xdata = value;
                OnPropertyChanged("xData");
            }
        }

        public TooltipSelectionMode? SelectionMode { get; set; }
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
