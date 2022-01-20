using Revit.DependencyInjection.Unity.Template.Commands.SampleViews.ViewModels;
using Revit.DependencyInjection.Unity.Template.Commands.SampleViews.Views;
using Revit.DependencyInjection.Unity.Template.Interfaces;
using Unity;

namespace Revit.DependencyInjection.Unity.Template
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