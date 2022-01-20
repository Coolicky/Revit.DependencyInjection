using System;
using System.Collections.Concurrent;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Base;
using Unity;

namespace Revit.DependencyInjection.Applications
{
    public abstract class RevitContainerProviderBase
    {
        
        internal static ConcurrentDictionary<string, IUnityContainer> Containers = new ConcurrentDictionary<string, IUnityContainer>();

        internal static IUnityContainer GetContainer(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new Exception($"Application should have a valid GUID to locate the container");
            }

            if (Containers.TryGetValue(guid, out IUnityContainer container))
            {
                return container;
            }

            throw new Exception($"No container found for this Application with specified GUID: {guid}");
        }

        internal IUnityContainer HookUpContainer(IUnityContainer container, string containerGuid)
        {
            // Do not duplicate existing container with the same GUID, instead, share same container
            if (Containers.ContainsKey(containerGuid))
            {
                container = Containers[containerGuid];
            }
            else
            {
                Containers[containerGuid] = container;
            }

            return container;
        }

        internal IUnityContainer InjectContainerToItself(IUnityContainer container)
        {
            
            container.RegisterInstance(container);
            return container;
        }

        internal IUnityContainer UnhookContainer(string containerGuid, IUnityContainer container)
        {
            container.Dispose();
            Containers.TryRemove(containerGuid, out _);
            return container;
        }

        internal IUnityContainer HookupRevitContext(UIControlledApplication application, IUnityContainer container)
        {
            var revitContext = new RevitContext();
            revitContext.HookupRevitEvents(application);
            container.RegisterInstance<IRevitContextProvider>(revitContext);
            container.RegisterInstance<IRevitContext>(revitContext);
            return container;
        }

        internal IUnityContainer HookupRevitContext(UIApplication application, IUnityContainer container)
        {
            var revitContext = new RevitContext();
            revitContext.HookupRevitEvents(application);
            container.RegisterInstance<IRevitContextProvider>(revitContext);
            container.RegisterInstance<IRevitContext>(revitContext);
            return container;
        }

        internal IUnityContainer UnhookRevitContext(UIControlledApplication application, string containerGuid)
        {
            try
            {
                var container = GetContainer(containerGuid);
                var revitContext = container.Resolve<IRevitContextProvider>();
                revitContext.UnhookRevitEvents(application);
                return container;
            }
            catch
            {
                return null;
            }
        }

        internal IUnityContainer UnhookRevitContext(UIApplication application, IUnityContainer container)
        {
            try
            {
                var revitContext = container.Resolve<IRevitContextProvider>();
                revitContext.UnhookRevitEvents(application);
                return container;
            }
            catch
            {
                return null;
            }
        }

        internal IUnityContainer AddRevitUI(IUnityContainer container, UIApplication application)
        {
            var revitUIApp = new RevitAppData
            {
                VersionBuild = application.Application.VersionBuild,
                VersionNumber = application.Application.VersionNumber,
                SubVersionNumber = application.Application.SubVersionNumber,
                VersionName = application.Application.VersionName,
            };

            container.RegisterInstance<IRevitAppData>(revitUIApp);
            return container;
        }

        internal IUnityContainer AddRevitUI(IUnityContainer container, UIControlledApplication application)
        {
            var revitUIApp = new RevitAppData
            {
                VersionBuild = application.ControlledApplication.VersionBuild,
                VersionNumber = application.ControlledApplication.VersionNumber,
                SubVersionNumber = application.ControlledApplication.SubVersionNumber,
                VersionName = application.ControlledApplication.VersionName,
            };

            container.RegisterInstance<IRevitAppData>(revitUIApp);
            return container;
        }
    }
}