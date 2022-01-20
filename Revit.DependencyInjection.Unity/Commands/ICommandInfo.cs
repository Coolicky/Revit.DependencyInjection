using System;
using Autodesk.Revit.UI;
using Unity;

namespace Revit.DependencyInjection.Unity.Commands
{
    /// <summary>
    /// Contains Information about the current running Revit Command.
    /// </summary>
    public interface ICommandInfo
    {
        /// <summary>
        /// Gets the type of the Revit Command.
        /// </summary>
        /// <returns>The type of the command.</returns>
        Type GetCommandType();
        /// <summary>
        /// Gets the command data from the Revit Command.
        /// </summary>
        /// <returns>The external command data.</returns>
        ExternalCommandData GetCommandData();
        /// <summary>
        /// Gets the current container from the Revit Command.
        /// </summary>
        /// <returns>Current Container.</returns>
        IUnityContainer GetContainer();
    }
}