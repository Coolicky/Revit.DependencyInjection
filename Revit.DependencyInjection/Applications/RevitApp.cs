﻿using System.ComponentModel;
using Revit.DependencyInjection.Base;
using Unity;

namespace Revit.DependencyInjection.Applications
{
    /// <summary>
    /// The base class implementation of RevitExternalAppBase using <see cref="Container"/> implementation for convinience
    /// <para>This class provides OnStartup, OnCreateRibbon, and OnShutdown lifecycle events.</para>
    /// <para>IMPORTANT: Any children of this class should implement <see cref="ContainerProviderAttribute"/> as well</para>
    /// </summary>
    public abstract class RevitApp : RevitAppBase<UnityContainer>
    {
    }
}