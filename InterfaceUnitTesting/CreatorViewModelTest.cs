using System.Windows.Media;
using Creator.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using ProbabilitySolver.Structs;
using Shared.Events;

namespace InterfaceUnitTesting
{
    [TestClass]
    public class CreatorViewModelTest
    {
        Brush _validBrush = new SolidColorBrush(Colors.LightGreen);
        Brush _inValidBrush = new SolidColorBrush(Colors.IndianRed);


        bool BrushEqual(Brush test, Brush expected)
        {
            var testBrush = (SolidColorBrush)test;
            var expectedBrush = (SolidColorBrush)expected;
            return (testBrush.Color == expectedBrush.Color);
        }

        CreatorViewModel MockCreatorViewModel()
        {
            var mockEventAggregator = new Moq.Mock<IEventAggregator>();
            mockEventAggregator.Setup(x => x.GetEvent<NewDistributionRequest>().Publish(new NewDistributionInfo()));
            return new CreatorViewModel(mockEventAggregator.Object);
        }


        void ChangeInputs(CreatorViewModel viewModel, DistributionEnums type, string infoOne, string infoTwo)
        {
            viewModel.SelectedType = type;
            viewModel.TypeChangedCommand.Execute();
            viewModel.InputOne = infoOne;
            viewModel.InputOneChangedCommand.Execute();
            viewModel.InputTwo = infoTwo;
            viewModel.InputTwoChangedCommand.Execute();

        }

        [TestMethod]
        public void InputBoxIsValidColourWithValidInput()
        {
            var creatorVM = MockCreatorViewModel();
            ChangeInputs(creatorVM,DistributionEnums.Binomial, "30", "0.3" );
            Assert.IsTrue(BrushEqual(creatorVM.InputOneBrush,_validBrush));
            Assert.IsTrue(BrushEqual(creatorVM.InputTwoBrush, _validBrush));

            ChangeInputs(creatorVM, DistributionEnums.Normal, "20", "20");
            Assert.IsTrue(BrushEqual(creatorVM.InputOneBrush, _validBrush)); 
            Assert.IsTrue(BrushEqual(creatorVM.InputTwoBrush, _validBrush));
        }


        [TestMethod]
        public void InputBoxIsInvalidColourWithInvalidInput()
        {
            var creatorVM = MockCreatorViewModel();
            ChangeInputs(creatorVM, DistributionEnums.Binomial, "200", "0.3");
            Assert.IsTrue(BrushEqual(creatorVM.InputOneBrush, _inValidBrush));
            Assert.IsTrue(BrushEqual(creatorVM.InputTwoBrush, _validBrush));

            ChangeInputs(creatorVM, DistributionEnums.Binomial, "-0.9", "2");
            Assert.IsTrue(BrushEqual(creatorVM.InputOneBrush, _inValidBrush));
            Assert.IsTrue(BrushEqual(creatorVM.InputTwoBrush, _inValidBrush));

            ChangeInputs(creatorVM, DistributionEnums.Normal, "50", "gg");
            Assert.IsTrue(BrushEqual(creatorVM.InputOneBrush, _validBrush));
            Assert.IsTrue(BrushEqual(creatorVM.InputTwoBrush, _inValidBrush));
        }

        [TestMethod]
        public void CreateButtonActiveWithValidInputs()
        {
            var creatorVM = MockCreatorViewModel();
            ChangeInputs(creatorVM,DistributionEnums.Binomial,"30","0.7");
            Assert.IsTrue(creatorVM.CreateButtonEnabled);
            ChangeInputs(creatorVM, DistributionEnums.Normal, "30", "100.8");
            Assert.IsTrue(creatorVM.CreateButtonEnabled);
        }

        [TestMethod]
        public void CreateButtonNotActiveWithInvalidInputs()
        {
            var creatorVM = MockCreatorViewModel();
            ChangeInputs(creatorVM, DistributionEnums.Binomial, "30.5", "0.7");
            Assert.IsFalse(creatorVM.CreateButtonEnabled);
            ChangeInputs(creatorVM, DistributionEnums.Binomial, "30", "1.2");
            Assert.IsFalse(creatorVM.CreateButtonEnabled);
            ChangeInputs(creatorVM, DistributionEnums.Normal, "", "1.2");
            Assert.IsFalse(creatorVM.CreateButtonEnabled);
        }
    }
}
