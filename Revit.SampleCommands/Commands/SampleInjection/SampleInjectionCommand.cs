using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Commands;
using Revit.SampleCommands.Interfaces;
using Unity;

namespace Revit.SampleCommands.Commands.SampleInjection
{
    [Transaction(TransactionMode.Manual)]
    public class SampleInjectionCommand : RevitAppCommand<App>
    {
        public override Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var sampleSelector = container.Resolve<ISampleSelector>();
            
            var selectedElements = sampleSelector.GetSelectedOrSelectElements().Result;
            var elementsMessage = string.Join("\n", 
                selectedElements.Select(r => $"Name: {r.Name} - ID: {r.Id}"));
            
            TaskDialog.Show("Selected Elements", elementsMessage);
            
            return Result.Succeeded;
        }
    }
}