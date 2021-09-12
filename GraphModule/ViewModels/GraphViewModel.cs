using LiveCharts;
using LiveCharts.Defaults;
using Prism.Events;
using Prism.Mvvm;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CSharpFunctionalExtensions;
using GraphModule.Views;
using LiveCharts.Wpf;

namespace GraphModule.ViewModels
{
    public class GraphViewModel : BindableBase
    {
        private GridRow _graphData;

        private IEnumerable<GridRow> data;

        private List<double> gridCases;
        private IEnumerable<double> gridProbability;
        private IEnumerable<double> gridSum;

        private ChartValues<ObservablePoint> probabilities;
        private ChartValues<ObservablePoint> sum;
        private ChartValues<ObservablePoint> bar;
        private int sigfigs = 4;
        private double minX;
        private double maxX;
        private double maxY;
        private string title;
        private IChartTooltip chartTooltip;

        public IChartTooltip ChartTooltip
        {
            get { return chartTooltip; }
            set { SetProperty(ref chartTooltip, value); }
        }
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public double MinX
        {
            get { return minX; }
            set { SetProperty(ref minX, value); }
        }

        public double MaxX
        {
            get { return maxX; }
            set { SetProperty(ref maxX, value); }
        }

        public double MaxY
        {
            get { return maxY; }
            set { SetProperty(ref maxY, value); }
        }

        public IEnumerable<GridRow> Data
        {
            get { return data; }
            set { SetProperty(ref data, value); }
        }

        public List<double> GridCases
        {
            get { return gridCases; }
            set { SetProperty(ref gridCases, value); }
        }

        public IEnumerable<double> GridProbability
        {
            get { return gridProbability; }
            set { SetProperty(ref gridProbability, value); }
        }

        public IEnumerable<double> GridSum
        {
            get { return gridSum; }
            set { SetProperty(ref gridSum, value); }
        }
        public ChartValues<ObservablePoint> Probabilities
        {
            get { return probabilities; }
            set { SetProperty(ref probabilities, value); }
        }
        public ChartValues<ObservablePoint> Sum
        {
            get { return sum; }
            set { SetProperty(ref sum, value); }
        }

        public ChartValues<ObservablePoint> Bar
        {
            get { return bar; }
            set { SetProperty(ref bar, value); }
        }

        private void GraphDeleted()
        {
            Data = null;
        }

        private List<List<double>> GetGridData(List<GridRow> dataStream)
        {
            var cases = new List<double>(dataStream.Count);
            var prob = new List<double>(dataStream.Count);
            var sum = new List<double>(dataStream.Count);

            foreach (var row in dataStream)
            {
                cases.Add(Math.Round(row.cases,sigfigs));
                prob.Add(Math.Round(row.probability,sigfigs));
                sum.Add(Math.Round(row.sumProbability,sigfigs));
            }
            return new List<List<double>>(){cases,prob,sum};
        }


        private bool IsDiscrete(List<GridRow> data)
        {
            return (data.Count > 0 && Math.Abs(data[1].cases - data[0].cases - 1f) < 0.0001);
        }

        private async Task GraphChanged(DistributionRow data)
        {
            if (data == null) 
                return;
            Title = data.name + "    " + data.info;

            var dS = new GridDataService();
            var dataStreamTask = await dS.GetAsync(data);
            if (dataStreamTask.IsFailure)
            {
                MessageBox.Show(dataStreamTask.Error);
                return;
            }

            var dataStream = dataStreamTask.Value;
            if (dataStream.Count == 0)
                return;
            Data = new List<GridRow>(511);
            
            Sum = new ChartValues<ObservablePoint>(dataStream.Select(dat => 
                new ObservablePoint(Math.Round(dat.cases,sigfigs), Math.Round(dat.sumProbability,sigfigs)*100)));

            var allGridData = GetGridData(dataStream);
            GridCases = allGridData[0];
            GridProbability = allGridData[1];
            GridSum = allGridData[2];

            if (IsDiscrete(dataStream))
            {
                Data = dataStream.Where(x => x.probability > 0.0011 || x.sumProbability == 1)
                    .Select(x => 
                        new GridRow(x.cases, Math.Round(x.probability, sigfigs), Math.Round(x.sumProbability, sigfigs) * 100));

                Bar = new ChartValues<ObservablePoint>(dataStream.Select
                    (dat => new ObservablePoint(dat.cases, Math.Round(dat.probability,4))));
                Probabilities = new ChartValues<ObservablePoint>();
                MaxY = dataStream.Max(x => x.probability) * 1.33;
                MaxX = dataStream.Max(x => x.cases);
                MinX = dataStream.First((x => x.sumProbability >= 0.001)).cases;
            }
            else
            {
                var sigfigOverride = sigfigs;
                for (int i = sigfigs; i < 9; i++)
                {
                    var magnitude = 10 ^ (-i+4);
                    if (!dataStream.Any(x => x.probability > magnitude))
                        sigfigOverride++;
                }
                Data = dataStream.Where(x => x.sumProbability > 0.0011 || x.sumProbability == 1).Select(x => 
                    new GridRow(Math.Round(x.cases,sigfigs), Math.Round(x.probability,sigfigOverride), Math.Round(x.sumProbability,sigfigs) * 100));
                Probabilities = new ChartValues<ObservablePoint>(dataStream.Select(dat => new ObservablePoint(Math.Round(dat.cases,sigfigs), Math.Round(dat.probability,sigfigOverride))));
                Bar = new ChartValues<ObservablePoint>();
                MaxY = dataStream.Max(x => x.probability) * 1.33;
                MaxX = dataStream.First((x => x.sumProbability >= 0.999)).cases;
                var tolerance = dataStream.First((x) => (x.sumProbability >= 0.005));
                var order = 10e-9;
                var lastOrder = order;

               var minTolerance = Math.Abs(tolerance.cases);

                while (order <minTolerance)
                {
                    lastOrder = order;
                    order *= 3;
                }

                var sgn = Math.Sign(tolerance.cases);
                if (sgn >0)
                    MinX = lastOrder*sgn;
                else
                {
                    MinX = order * sgn;
                }
            }
        }

        public GraphViewModel(IEventAggregator ea)
        {
            
            ea.GetEvent<DistributionChangedEvent>().Subscribe(async data => await GraphChanged(data));
            ea.GetEvent<DistributionDeletedEvent>().Subscribe((data) => GraphDeleted());


            MaxX = 100;
            MaxY = 1;

            var cases = new List<double>();
            var probabilities = new List<double>();
            var sumProbability = new List<double>();

            Probabilities = new ChartValues<ObservablePoint>();
            Sum = new ChartValues<ObservablePoint>();
            ChartTooltip = new ChartToolTipView();
        }
    }
}
