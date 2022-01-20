using Revit.DependencyInjection.Abstractions;
using Revit.SampleCommands.Commands.SampleViews.ViewModels;
using Revit.SampleCommands.Commands.SampleViews.Views;
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
        
        public static IUnityContainer RegisterViews(this IUnityContainer container)
        {
            container.RegisterType<SampleWindow>();
            return container;
        }
        
        public static IUnityContainer RegisterViewModels(this IUnityContainer container)
        {
            container.RegisterType<SampleWindowViewModel>();
            return container;
        }
    }
}