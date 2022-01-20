using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.UI;
using Unity;

namespace Revit.DependencyInjection.Unity.Applications
{
    public interface IRevitExternalApp : IExternalApplication
    {
        /// <summary>
        /// Lifecycle hook to create Ribbon UI when Revit starts.
        /// </summary>
        void OnCreateRibbon(IRibbonManager ribbonManager);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        Result OnShutdown(IUnityContainer container, UIControlledApplication application);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        Result OnStartup(IUnityContainer container, UIControlledApplication application);
    }
}