using Prism.Mvvm;
using ProbabilitySolver.Structs;

namespace ProbabilityMenu.ViewModels
{
    public class MenuButtonViewModel : BindableBase
    {
        public string name { get; set; }
        public string info { get; set; }

        public DistributionRow distributionData { get; set; }

        public MenuButtonViewModel(DistributionRow dD)
        {
            this.name = dD.name;
            this.info = dD.info;
            this.distributionData = dD;
        }
    }
}
