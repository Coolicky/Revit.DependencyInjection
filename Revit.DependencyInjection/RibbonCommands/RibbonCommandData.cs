using Autodesk.Revit.UI;
using Revit.DependencyInjection.RibbonCommands.Attributes;

namespace Revit.DependencyInjection.RibbonCommands
{
    internal class RibbonCommandData
    {
        internal RibbonPanel ribbonPanel;
        internal PushButtonData button;
        internal IRibbonCommandAttribute commandAttribute;
    }
}