using Autodesk.Revit.UI;
using Revit.DependencyInjection.Base;
using Revit.DependencyInjection.Commands;
using Revit.DependencyInjection.Commands.ErrorHandlers;
using Revit.DependencyInjection.UI;
using Unity;

namespace Revit.DependencyInjection.Applications
{
    public abstract class RevitAppBase<TContainer> : RevitContainerProviderBase, IRevitExternalApp where TContainer : class, IUnityContainer, new()
    {
        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        public abstract Result OnStartup(IUnityContainer container, UIControlledApplication application);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            var containerGuid = ContainerProviderReflector.GetContainerGuid(this);

            var container = new TContainer();
            InjectContainerToItself(container);

            HookUpContainer(container, containerGuid);
            HookupRevitContext(application, container);
            AddRevitUI(container, application);

            container.AddRevitCommandGuardConditions();
            container.AddRevitCommandErrorHandling<EmptyRevitCommandErrorHandler>();

            // Calls the client's Startup
            var result = OnStartup(container, application);
            if (result != Result.Succeeded)
            {
                return result;
            }

            // Calls the client's CreateRibbon
            var imageManager = new ImageManager();
            var ribbonManager = new RibbonManager(application, imageManager);
            OnCreateRibbon(ribbonManager);

            return result;
        }

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        public abstract Result OnShutdown(IUnityContainer container, UIControlledApplication application);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
            var containerGuid = ContainerProviderReflector.GetContainerGuid(this);
            var container = Containers[containerGuid];

            try
            {
                // Unhooks the events
                UnhookRevitContext(application, containerGuid);
                // Calls the client's Shutdown
                return OnShutdown(container, application);
            }
            finally
            {
                // Unhooks and cleans the container even if an exception is thrown
                UnhookContainer(containerGuid, container);
            }
        }

        /// <summary>
        /// Lifecycle hook to create Ribbon UI when Revit starts.
        /// </summary>
        public virtual void OnCreateRibbon(IRibbonManager ribbonManager)
        {
        }
    }
}