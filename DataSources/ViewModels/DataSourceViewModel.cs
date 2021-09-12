using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Prism.Events;
using ProbabilitySolver.Distributions;
using ProbabilitySolver.Services.FileService;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace DataSources.ViewModels
{
    public class DataSourceViewModel : BindableBase
    {

        private string _SQLServerName;

        private  string _FullPathString;

        private IEventAggregator _ea;

        private Brush dataBaseButtonBrush;

        public Brush DataBaseButtonBrush
        {
            get { return dataBaseButtonBrush; }
            set { SetProperty(ref dataBaseButtonBrush, value); }
        }

        public string FullPathString
        {
            get { return _FullPathString; }
            set { SetProperty(ref _FullPathString, value); }
        }

        public string SQLServerName
        {
            get { return _SQLServerName; }
            set { SetProperty(ref _SQLServerName, value); }
        }

        public DelegateCommand TestConnectionCommand { get; private set; }
        public DelegateCommand CreateDatabaseCommand { get; private set; }

        public DelegateCommand SaveCsvCommand { get; private set; }


        public void CreateDatabase()
        {
            var sqlServiceSetup = new SQLServiceSetup(SQLServerName);
            var createDB = sqlServiceSetup.Create();
            string diagnostic = "Success!";
            if (createDB.IsFailure)
            {
                diagnostic = createDB.Error;
                MessageBox.Show(diagnostic);
                return;
            }
            sqlServiceSetup.SaveSqlConfig(SQLServerName);
            _ea.GetEvent<NewDataBaseEvent>().Publish();
            MessageBox.Show(diagnostic);
        }

        public void TestConnection()
        {
            var sqlServiceSetup = new SQLServiceSetup(SQLServerName);
            var testDB = sqlServiceSetup.TestConnection();
            string diagnostic = "Test Connection Successful";
            if (testDB.IsFailure)
                diagnostic = testDB.Error;
            MessageBox.Show(diagnostic);
        }

        public void SetDirectory()
        {
            // Default to Documents folder
            var csvService = new CsvService();
            var setDirectory = csvService.ChangeDirectory(this.FullPathString);
            if (setDirectory.IsFailure)
            {
                MessageBox.Show(setDirectory.Error);
            }

            else
            {
                MessageBox.Show("CSVs save to: " + this.FullPathString);
            }

        }

        public DataSourceViewModel(IEventAggregator ea)
        {
            var csvHelper = new CsvService();
            var getCSVDirectory = csvHelper.GetSavePath();
            FullPathString = getCSVDirectory.IsSuccess ? getCSVDirectory.Value : string.Empty;
            var sqlHelper = new SQLServiceSetup();
            var getSqlConfig = sqlHelper.GetSqlConfig();
            SQLServerName = getSqlConfig.IsSuccess ? getSqlConfig.Value : string.Empty;
            this._ea = ea;
            CreateDatabaseCommand = new DelegateCommand(() => CreateDatabase());
            TestConnectionCommand = new DelegateCommand((() => TestConnection()));
            SaveCsvCommand = new DelegateCommand(SetDirectory);

        }
       
    }
}
