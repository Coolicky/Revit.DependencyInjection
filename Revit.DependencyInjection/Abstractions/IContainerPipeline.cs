﻿using Unity;

namespace Revit.DependencyInjection.Abstractions
{
    /// <summary>
    /// Container pipeline is used to add dependencies and compose a container
    /// </summary>
    public interface IContainerPipeline
    {
        /// <summary>
        /// Adds dependencies to the container and returns it
        /// </summary>
        IUnityContainer Pipe(IUnityContainer container);
    }
}