using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Applications;
using Revit.DependencyInjection.Base;
using Revit.DependencyInjection.Commands.ErrorHandlers;
using Revit.DependencyInjection.Commands.Guards;
using Unity;

namespace Revit.DependencyInjection.Commands
{
    /// <summary>
    /// Base class to implement Revit Commands linked to a App Container
    /// <br>It will use a scope of the container declared on the App</br>
    /// </summary>
    public abstract class RevitAppCommand<TApplication> : IExternalCommand, ICanBeGuardedRevitCommand, IRevitDestroyableCommand where TApplication : RevitApp, new()
    {
        /// <summary>
        /// Execution of External Command
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var commandType = GetType();

            // Gets the original container
            IUnityContainer container = GetContainer();

            // Creates an scoped copy of the container
            var scope = container.CreateChildContainer();

            var commandInfo = new CommandInfo(commandType, scope, commandData);

            try
            {
                var commandGuardChecker = scope.Resolve<IRevitCommandGuardChecker>();

                if (commandGuardChecker.CanExecute(commandInfo))
                {
                    // Runs the users Execute command
                    return Execute(scope, commandData, ref message, elements);
                }
                else
                {
                    return Result.Cancelled;
                }
            }
            catch (Exception exception)
            {
                var errorHandler = scope.Resolve<IRevitCommandErrorHandler>();
                // If an exception is thrown on user's code, and the handler doesnt handle it, throw the except it back to the stack
                if (!errorHandler.Handle(commandInfo, exception))
                {
                    throw;
                }
                else
                {
                    // If the handler handles the exception, the command will return succeeded
                    return Result.Succeeded;
                }
            }
            finally
            {
                // Safely calls lifecycle hook 
                try
                {
                    OnDestroy(scope);
                }
                catch
                {
                    // ignored
                }

                // Cleans up the scoped copy of the container
                scope.Dispose();
            }
        }

        private static IUnityContainer GetContainer()
        {
            var type = typeof(TApplication);
            var containerGuid = ContainerProviderReflector.GetContainerGuid(type);
            var container = RevitContainerProviderBase.GetContainer(containerGuid);
            return container;
        }

        /// <summary>
        /// Execution of External Command
        /// </summary>
        public abstract Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message, ElementSet elements);

        /// <summary>
        /// External Command lifecycle hook which is called just before the scoped container is disposed.
        /// </summary>
        public virtual void OnDestroy(IUnityContainer container)
        {
        }
    }
}