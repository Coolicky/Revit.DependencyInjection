using System;
using System.Collections.Generic;

namespace Revit.DependencyInjection.Unity.Commands.Guards
{
    internal interface IConditionBuilderProvider
    {
        Predicate<ICommandInfo> GetPredicate();
        List<Type> GetCommandTypes();
    }
}