using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.RibbonCommands.Attributes;

namespace Revit.DependencyInjection.Unity.RibbonCommands
{
    internal class RibbonCommandData
    {
        internal RibbonPanel RibbonPanel;
        internal PushButtonData Button;
        internal IRibbonCommandAttribute CommandAttribute;
    }
}