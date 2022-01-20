using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Base
{
    public interface IRevitContextProvider
    {
        
        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        void HookupRevitEvents(UIControlledApplication application);
        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        void UnhookRevitEvents(UIControlledApplication application);
        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        void HookupRevitEvents(UIApplication application);
        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        void UnhookRevitEvents(UIApplication application);
    }
}