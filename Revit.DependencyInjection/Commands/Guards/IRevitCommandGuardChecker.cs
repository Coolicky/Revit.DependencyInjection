using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Revit.DependencyInjection.Commands.Attributes;

namespace Revit.DependencyInjection.Commands.Guards
{
    public interface IRevitCommandGuardChecker
    {
        bool CanExecute(ICommandInfo commandInfo);
    }
    
    public class RevitCommandGuardChecker : IRevitCommandGuardChecker
    {
        private readonly Dictionary<Type, List<Predicate<ICommandInfo>>> _commandConditions;

        public RevitCommandGuardChecker()
        {
            _commandConditions = new Dictionary<Type, List<Predicate<ICommandInfo>>>();
        }

        public bool CanExecute(ICommandInfo commandInfo)
        {
            var commandType = commandInfo.GetCommandType();

            if (!CheckCommandGuardAttributesIfCommandCanExecute(commandInfo, commandType)) return false;

            var ignoreConditionsType = typeof(IgnoreCommandGuardConditionsAttribute);
            var attributeData = commandType.CustomAttributes;

            // If IgnoreConditions is added to the command, we wont check conditions, just allow the command to run
            if (attributeData.Any(a => a.AttributeType == ignoreConditionsType))
            {
                return true;
            }

            // Loop through all conditions to see if we can run the command
            if (!_commandConditions.ContainsKey(commandType)) return true;
            var conditions = _commandConditions[commandType];
            foreach (var condition in conditions)
            {
                var canExecute = condition.Invoke(commandInfo);
                if (!canExecute)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckCommandGuardAttributesIfCommandCanExecute(ICommandInfo commandInfo, MemberInfo commandType)
        {
            // Loop through all RevitCommandGuardAttributes to see if we can run the command
            var guardAttrType = typeof(CommandGuardAttribute);
            var attributes = commandType
                .GetCustomAttributes()
                .Where(a => a.GetType() == guardAttrType)
                .ToList();
            if (!attributes.Any()) return true;
            var methodInfo = guardAttrType.GetMethod(nameof(CommandGuardAttribute.GetCommandGuardType));
            foreach (var attribute in attributes)
            {
                if (methodInfo == null) continue;
                if (!(methodInfo.Invoke(attribute, null) is Type guardType)) continue;
                if (!(Activator.CreateInstance(guardType) is IRevitCommandGuard guard)) continue;
                if (!guard.CanExecute(commandInfo))
                {
                    return false;
                }
            }

            return true;
        }

        internal void AddCommandTypeCondition(Type commandType, Predicate<ICommandInfo> predicate)
        {
            if (_commandConditions.ContainsKey(commandType))
            {
                var commandCondition = _commandConditions[commandType];
                commandCondition.Add(predicate);
            }
            else
            {
                _commandConditions.Add(commandType, new List<Predicate<ICommandInfo>> { predicate });
            }
        }
    }
}