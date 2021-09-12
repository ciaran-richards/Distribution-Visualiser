using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Creator.ViewModels;
using CSharpFunctionalExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using ProbabilityMenu.ViewModels;
using ProbabilitySolver.Services.FileService;
using ProbabilitySolver.Services.SQLService;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace InterfaceUnitTesting
{
    [TestClass]
    public class MenuViewModelTest
    {
        private const string csvNamePredicate = "CSVTestOne";
        private const string dbNamePredicate = "DBTestOne";
        List<DistributionRow> _sqlList = new List<DistributionRow>()
        {
            new DistributionRow(dbNamePredicate, "I[0,0]", Guid.Empty, Guid.Empty),
            new DistributionRow("DBTestTwo", "I[0,1]", Guid.Empty, Guid.Empty)
        };

        List<DistributionRow> _csvList = new List<DistributionRow>()
        {
            new DistributionRow(csvNamePredicate, "I[1,0]", Guid.Empty, Guid.Empty),
            new DistributionRow("CSVTestTwo", "I[1,1]", Guid.Empty, Guid.Empty),
            new DistributionRow("CSVTestThree", "I[1,2]", Guid.Empty, Guid.Empty),
        };

        public MenuViewModel MockMenuViewModel(bool useDB, bool useCSV)
        {
            // Initialise View Model
            var mockEventAggregator = new Moq.Mock<IEventAggregator>();
            mockEventAggregator.Setup(x => x.GetEvent<DistributionAddedEvent>().Publish(
                new DistributionRow("", "", Guid.Empty, Guid.Empty)));
            mockEventAggregator.Setup(x => x.GetEvent<NewDataBaseEvent>().Publish());
            mockEventAggregator.Setup(x => x.GetEvent<RefreshEvent>().Publish());
            var mockedViewModel = new MenuViewModel(mockEventAggregator.Object);

            
            //Configure SQL Response
            var mockDistributionService = new Moq.Mock<IDistributionQuery>();
            mockDistributionService.Setup(x => x.GetAllDistributions()).Returns( useDB ? Result.Success(
                    new List<DistributionRow>(_sqlList)) : Result.Failure<List<DistributionRow>>("SQLTest"));
            mockedViewModel.Distribution = mockDistributionService.Object;

            //Configure CSV Response
            var mockCsvService = new Moq.Mock<IDistributionQuery>();
            mockCsvService.Setup(x => x.GetAllDistributions()).Returns( useCSV ? Result.Success(
                    new List<DistributionRow>(_csvList)) : Result.Failure<List<DistributionRow>>("CSVTest"));
            mockedViewModel.Csv = mockCsvService.Object;

            mockedViewModel.Refresh();
            return mockedViewModel;
        }

        [TestMethod]
        public void ShowMenuWithDatabaseOnly()
        {
            var menuViewModel = MockMenuViewModel(true, false);
            var menuListTest = menuViewModel.ItemCollection;
            Assert.AreEqual(menuListTest.Count(), 2);
            Assert.IsTrue(menuListTest.Any(x => x.name == dbNamePredicate));
        }

        [TestMethod]
        public void ShowMenuWithCSVOnly()
        {
            var menuViewModel = MockMenuViewModel(false, true);
            var menuListTest = menuViewModel.ItemCollection;
            int test = menuListTest.Count();
            Assert.AreEqual(3, test);
            Assert.IsTrue(menuListTest.Any(x => x.name == csvNamePredicate));
        }

        [TestMethod]
        public void ShowMenuWithAllSources()
        {
            var menuViewModel = MockMenuViewModel(true, true);
            var menuListTest = menuViewModel.ItemCollection;
            int test = menuListTest.Count();
            Assert.AreEqual(5, test);
            Assert.IsTrue(menuListTest.Any(x => x.name == dbNamePredicate));
            Assert.IsTrue(menuListTest.Any(x => x.name == csvNamePredicate));
        }
    }
}
