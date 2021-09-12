using Ribbon.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shared;

namespace Ribbon
{
    public class RibbonModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public RibbonModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion((RegionNames.RibbonRegion), typeof(RibbonView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}