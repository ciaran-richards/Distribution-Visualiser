using ProbabilityMenu.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shared;

namespace ProbabilityMenu
{
    public class MenuModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion((RegionNames.MenuRegion), typeof(MenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}