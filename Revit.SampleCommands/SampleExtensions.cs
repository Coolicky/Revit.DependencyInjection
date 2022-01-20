using Revit.DependencyInjection.Abstractions;
using Revit.SampleCommands.Interfaces;
using Unity;

namespace Revit.SampleCommands
{
    public static class SamplePipeline
    {
        public static IUnityContainer RegisterSampleServices(this IUnityContainer container)
        {
            container.RegisterType<ISampleSelector, SampleSelector>();
            return container;
        }
    }
}