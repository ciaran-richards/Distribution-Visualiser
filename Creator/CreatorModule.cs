using Creator.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shared;

namespace Creator
{
    public class CreatorModule : IModule
    {

        private readonly IRegionManager _regionManager;

        public CreatorModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion((RegionNames.CreatorRegion), typeof(CreatorView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}