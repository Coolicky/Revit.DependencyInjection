using System;
using Autodesk.Revit.UI;
using Unity;

namespace Revit.DependencyInjection.Unity.Commands
{
    public class CommandInfo : ICommandInfo
    {
        private readonly Type _commandType;
        private readonly IUnityContainer _container;
        private readonly ExternalCommandData _commandData;

        internal CommandInfo(Type commandType, IUnityContainer container, ExternalCommandData commandData)
        {
            _commandType = commandType;
            _container = container;
            _commandData = commandData;
        }

        public IUnityContainer GetContainer()
        {
            return _container;
        }

        public Type GetCommandType()
        {
            return _commandType;
        }

        public ExternalCommandData GetCommandData()
        {
            return _commandData;
        }
    }
}