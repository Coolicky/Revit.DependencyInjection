using System;
using Revit.DependencyInjection.Unity.Commands.ErrorHandlers;
using Revit.DependencyInjection.Unity.Commands.Guards;
using Unity;

namespace Revit.DependencyInjection.Unity.Commands
{
    /// <summary>
    /// Command Extensions
    /// </summary>
    public static class CommandExtensions
    {
        static internal IUnityContainer AddRevitCommandGuardConditions(this IUnityContainer container)
        {
            var commandGuardChecker = new RevitCommandGuardChecker();
            container.RegisterInstance(commandGuardChecker);
            container.RegisterInstance<IRevitCommandGuardChecker>(commandGuardChecker);

            return container;
        }

        /// <summary>
        /// Adds support for guarding Revit Commands with Guard Conditions.
        /// </summary>
        /// <param name="container">The current container.</param>
        /// <param name="configuration">The configuration to add guard conditions.</param>
        /// <returns>The container.</returns>
        static public IUnityContainer AddRevitCommandGuardConditions(this IUnityContainer container, Action<IConditionCollection> configuration)
        {
            var commandGuardChecker = new RevitCommandGuardChecker();
            container.RegisterInstance(commandGuardChecker);
            container.RegisterInstance<IRevitCommandGuardChecker>(commandGuardChecker);

            var collection = new ConditionCollection();
            configuration?.Invoke(collection);

            var builders = collection.GetBuilders();

            foreach (var builder in builders)
            {
                var commandTypes = builder.GetCommandTypes();
                var predicate = builder.GetPredicate();

                foreach (var commandType in commandTypes)
                {
                    commandGuardChecker.AddCommandTypeCondition(commandType, predicate);
                }
            }

            return container;
        }

        /// <summary>
        /// Adds support for handling exceptions globally in Revit Commands.
        /// </summary>
        /// <typeparam name="THandler">A class that inherits from <see cref="IRevitCommandErrorHandler"/> to handle the exceptions.</typeparam>
        /// <param name="container">The current container.</param>
        /// <returns>The Container.</returns>
        static public IUnityContainer AddRevitCommandErrorHandling<THandler>(this IUnityContainer container) where THandler : class, IRevitCommandErrorHandler, new()
        {
            var hanlder = new THandler();
            container.RegisterInstance(hanlder);
            container.RegisterSingleton<IRevitCommandErrorHandler, THandler>();

            return container;
        }
    }
}