using GraphModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Shared;

namespace GraphModule.Modules
{
    public class GraphModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public GraphModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion((RegionNames.GraphRegion), typeof(GraphView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

    }
}