using System;
using Unity;

namespace Revit.DependencyInjection.Async
{
    /// <summary>
    /// Extensions methods to adding support for Async calls in Revit
    /// </summary>
    public static class RevitAsyncExtensions
    {
        /// <summary>
        /// Adds support for making async calls to Revit 
        /// </summary>
        public static void AddRevitAsync(this IUnityContainer container, Action<RevitAsyncSettings> config)
        {
            if (config == null)
            {
                throw new ArgumentException("Adding Revit Async should have a valid configuration.");
            }

            var settings = new RevitAsyncSettings();
            config.Invoke(settings);

            if (string.IsNullOrWhiteSpace(settings.Name))
            {
                throw new ArgumentException("Adding Revit Async should have a valid handler name.");
            }

            container.RegisterInstance(settings);
            container.RegisterSingleton<IRevitEventHandler, RevitExternalHandler>();
        }
    }
}