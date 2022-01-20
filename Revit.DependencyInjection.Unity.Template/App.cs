using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.Applications;
using Revit.DependencyInjection.Unity.Async;
using Revit.DependencyInjection.Unity.Base;
using Revit.DependencyInjection.Unity.Template.Commands.Availability;
using Revit.DependencyInjection.Unity.Template.Commands.HelloWorld;
using Revit.DependencyInjection.Unity.Template.Commands.SampleInjection;
using Revit.DependencyInjection.Unity.Template.Commands.SampleViews;
using Revit.DependencyInjection.Unity.UI;
using Unity;

namespace Revit.DependencyInjection.Unity.Template
{
    [ContainerProvider("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            var br = ribbonManager.GetLineBreak();

            var sampleTab = ribbonManager.CreateTab("Sample Tab");
            var samplePanel = ribbonManager.CreatePanel(sampleTab.GetTabName(), "Sample Panel");
            
            samplePanel.AddPushButton<HelloWorldCommand, AvailableAlways>($"Hello{br}World", "hello");
            samplePanel.AddPushButton<SampleInjectionCommand, AvailableOnProject>($"Get{br}Selection", "selection");
            samplePanel.AddPushButton<SampleWindowCommand, AvailableOnProject>($"Show{br}Window", "window");
        }

        public override Result OnStartup(IUnityContainer container, UIControlledApplication application)
        {
            container.AddRevitAsync(GetAsyncSettings);
            container.RegisterSampleServices();
            container.RegisterViews();
            container.RegisterViewModels();
            return Result.Succeeded;
        }

        public override Result OnShutdown(IUnityContainer container, UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        
        private void GetAsyncSettings(RevitAsyncSettings settings)
        {
            settings. Name = "Revit DI Samples";
            settings.IsJournalable = true;
        }
    }
}