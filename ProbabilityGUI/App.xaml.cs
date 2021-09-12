using ProbabilityGUI.Views;
using Prism.Ioc;
using System.Windows;
using System.Windows.Controls;
using Creator;
using Creator.Views;
using Creator.Windows;
using DataSources;
using Prism.Modularity;
using ProbabilityMenu;
using Ribbon;

namespace ProbabilityGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            Container.Resolve<CreatorWindow>();
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MenuModule>();
            moduleCatalog.AddModule<RibbonModule>();
            moduleCatalog.AddModule<GraphModule.Modules.GraphModule>();
            moduleCatalog.AddModule<CreatorModule>();
            moduleCatalog.AddModule<DataSourcesModule>();
        }


    }
}
