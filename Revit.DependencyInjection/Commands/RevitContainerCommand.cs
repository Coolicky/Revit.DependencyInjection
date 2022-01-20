using Revit.DependencyInjection.Abstractions;
using Unity;

namespace Revit.DependencyInjection.Commands
{
    /// <summary>
    /// An indepentend Revit Command that will create a new container instance and use it during the command runtime. Use this when an ExternalApplication is not necessary.
    /// <br>It uses a Container Pipeline to compose the container.</br>
    /// <br>After the command finishes the container will be disposed.</br>
    /// </summary>
    public abstract class
        RevitContainerCommand<TContainerPipeline> : RevitContainerCommandBase<TContainerPipeline, UnityContainer>
        where TContainerPipeline : class, IContainerPipeline, new()
    {
    }
}