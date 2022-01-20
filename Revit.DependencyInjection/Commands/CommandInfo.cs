using System;
using Autodesk.Revit.UI;
using Unity;

namespace Revit.DependencyInjection.Commands
{
    public class CommandInfo : ICommandInfo
    {
        private readonly Type commandType;
        private readonly IUnityContainer container;
        private readonly ExternalCommandData commandData;

        internal CommandInfo(Type commandType, IUnityContainer container, ExternalCommandData CommandData)
        {
            this.commandType = commandType;
            this.container = container;
            commandData = CommandData;
        }

        public IUnityContainer GetContainer()
        {
            return container;
        }

        public Type GetCommandType()
        {
            return commandType;
        }

        public ExternalCommandData GetCommandData()
        {
            return commandData;
        }
    }
}