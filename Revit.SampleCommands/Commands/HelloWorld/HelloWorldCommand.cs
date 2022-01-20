using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Commands;
using Unity;

namespace Revit.SampleCommands.Commands.HelloWorld
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorldCommand : RevitAppCommand<App>
    {
        public override Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Hello", "Hello World");
            
            return Result.Succeeded;
        }
    }
}