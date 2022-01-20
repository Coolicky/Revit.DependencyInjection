using Autodesk.Revit.UI;
using Revit.DependencyInjection.RibbonCommands.Attributes;

namespace Revit.DependencyInjection.RibbonCommands
{
    internal class RibbonCommandData
    {
        internal RibbonPanel RibbonPanel;
        internal PushButtonData Button;
        internal IRibbonCommandAttribute CommandAttribute;
    }
}