using System;

namespace Revit.DependencyInjection.Commands.ErrorHandlers
{
    internal class EmptyRevitCommandErrorHandler : IRevitCommandErrorHandler
    {
        public bool Handle(ICommandInfo commandInfo, Exception exception)
        {
            return false;
        }
    }
}