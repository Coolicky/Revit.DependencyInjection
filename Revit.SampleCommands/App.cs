using Autodesk.Revit.UI;
using Revit.DependencyInjection.Applications;
using Revit.DependencyInjection.Async;
using Revit.DependencyInjection.Base;
using Revit.DependencyInjection.UI;
using Revit.SampleCommands.Commands.Availability;
using Revit.SampleCommands.Commands.HelloWorld;
using Revit.SampleCommands.Commands.SampleInjection;
using Revit.SampleCommands.Commands.SampleViews;
using Unity;

namespace Revit.SampleCommands
{
    [ContainerProvider("EE9FD955-E885-4E9C-87D0-A77F2F6585A9")]
    public class App : RevitApp
    {
        public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
            var br = ribbonManager.GetLineBreak();

            var sampleTab = ribbonManager.CreateTab("Sample Tab");
            var samplePanel = ribbonManager.CreatePanel(sampleTab.GetTabName(), "Sample Panel");
            
            samplePanel.AddPushButton<HelloWorldCommand, AvailableAlways>($"Hello{br}World");
            samplePanel.AddPushButton<SampleInjectionCommand, AvailableOnProject>($"Get{br}Selection");
            samplePanel.AddPushButton<SampleWindowCommand, AvailableOnProject>($"Show{br}Window");
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