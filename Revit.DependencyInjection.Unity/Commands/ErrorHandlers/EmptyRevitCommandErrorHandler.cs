using System;

namespace Revit.DependencyInjection.Unity.Commands.ErrorHandlers
{
    internal class EmptyRevitCommandErrorHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            return false;
        }
    }
}