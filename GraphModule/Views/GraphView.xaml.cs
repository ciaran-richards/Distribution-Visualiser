﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using ProbabilitySolver.Structs;

namespace GraphModule.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        public GraphView()
        {
            InitializeComponent();
        }

        CartesianMapper<GridRow> SumMapper => new CartesianMapper<GridRow>().X(row => row.cases).Y(row => row.sumProbability);
        CartesianMapper<GridRow> ProbabilityMapper => new CartesianMapper<GridRow>().X(row => row.cases).Y(row => row.probability);

        private void MyGrid_OnAutoGeneratedColumns(object sender, EventArgs e)
        {
            MyGrid.Columns[0].Header = "Case";
            MyGrid.Columns[1].Header = "Probability";
            MyGrid.Columns[2].Header = "Cumulative %";
        }
    }
}