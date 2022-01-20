using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.Commands;
using Revit.DependencyInjection.Unity.Template.Commands.SampleViews.Views;
using Unity;

namespace Revit.DependencyInjection.Unity.Template.Commands.SampleViews
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