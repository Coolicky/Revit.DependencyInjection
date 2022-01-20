using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Abstractions;
using Revit.DependencyInjection.Applications;
using Revit.DependencyInjection.Commands.ErrorHandlers;
using Revit.DependencyInjection.Commands.Guards;
using Unity;

namespace Revit.DependencyInjection.Commands
{
    /// <summary>
    /// An independent Revit Command that will create a new container instance and use it during the command runtime. Use this when an ExternalApplication is not necessary.
    /// <br>It uses a Container Pipeline to compose the container.</br>
    /// <br>After the command finishes the container will be disposed.</br>
    /// </summary>
    public abstract class RevitContainerCommandBase<TContainerPipeline, TContainer> : RevitContainerProviderBase,
        IExternalCommand, ICanBeGuardedRevitCommand,
        IRevitDestroyableCommand where TContainerPipeline : class, IContainerPipeline, new()
        where TContainer : class, IUnityContainer, new()
    {
        /// <summary>
        /// Execution of External Command.
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var commandType = GetType();

            var pipeline = new TContainerPipeline();
            var container = new TContainer();
            var application = commandData.Application;

            InjectContainerToItself(container);

            HookupRevitContext(application, container);
            AddRevitUI(container, application);

            // Add Default Guard Conditions and Error Handling before piping
            container.AddRevitCommandGuardConditions();
            container.AddRevitCommandErrorHandling<EmptyRevitCommandErrorHandler>();

            var newContainer = pipeline.Pipe(container);

            var commandInfo = new CommandInfo(commandType, newContainer, commandData);

            try
            {
                // Needs to resolve Command Guard because the pipeline could have changed it
                var commandGuardChecker = newContainer.Resolve<IRevitCommandGuardChecker>();

                if (commandGuardChecker.CanExecute(commandInfo))
                {
                    // Runs the users Execute command
                    return Execute(newContainer, commandData, ref message, elements);
                }
                else
                {
                    return Result.Cancelled;
                }
            }
            catch (Exception exception)
            {
                // Needs to resolve Command handler because the pipeline could have changed it
                var errorHandler = newContainer.Resolve<IRevitCommandErrorHandler>();

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
                UnhookRevitContext(application, container);
                // Safely calls lifecycle hook 
                try
                {
                    OnDestroy(newContainer);
                }
                catch
                {
                    // ignored
                }

                // Cleans up the container
                container.Dispose();
            }
        }

        /// <summary>
        /// Execution of External Command.
        /// </summary>
        public abstract Result Execute(IUnityContainer container, ExternalCommandData commandData, ref string message,
            ElementSet elements);

        /// <summary>
        /// External Command lifecycle hook which is called just before the container is disposed.
        /// </summary>
        public virtual void OnDestroy(IUnityContainer container)
        {
        }
    }
}