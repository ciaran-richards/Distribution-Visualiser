using DataSources.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shared;

namespace DataSources
{
    public class DataSourcesModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public DataSourcesModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion((RegionNames.DataSourcesRegion), typeof(DataSourceView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}