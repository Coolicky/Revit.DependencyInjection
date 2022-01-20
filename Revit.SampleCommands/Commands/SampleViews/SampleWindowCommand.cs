using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Commands;
using Revit.SampleCommands.Commands.SampleViews.Views;
using Unity;

namespace Revit.SampleCommands.Commands.SampleViews
{
    [Transaction(TransactionMode.Manual)]
    public class SampleWindowCommand : RevitAppCommand<App>
    {
        public override Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var window = container.Resolve<SampleWindow>();
            window.Show();
            
            return Result.Succeeded;
        }
    }
}