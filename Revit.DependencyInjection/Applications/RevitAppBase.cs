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
            this.InjectContainerToItself(container);

            this.HookUpContainer(container, containerGuid);
            this.HookupRevitContext(application, container);
            this.AddRevitUI(container, application);

            container.AddRevitCommandGuardConditions();
            container.AddRevitCommandErrorHandling<EmptyRevitCommandErrorHandler>();

            try
            {
                // Calls the client's Startup
                var result = this.OnStartup(container, application);
                if (result != Result.Succeeded)
                {
                    return result;
                }

                // Calls the client's CreateRibbon
                var imageManager = new ImageManager();
                var ribbonManager = new RibbonManager(application, imageManager);
                this.OnCreateRibbon(ribbonManager);

                return result;
            }
            catch
            {
                // If an exception the client's code, throw the exception to the stack
                throw;
            }
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
            var container = containers[containerGuid];

            try
            {
                // Unhooks the events
                this.UnhookRevitContext(application, containerGuid);
                // Calls the client's Shotdown
                return this.OnShutdown(container, application);
            }
            catch
            {
                // If anything goes wrong with the client's code, throw the exception to the stack
                throw;
            }
            finally
            {
                // Unhooks and cleans the container even if an exception is thrown
                this.UnhookContainer(containerGuid, container);
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