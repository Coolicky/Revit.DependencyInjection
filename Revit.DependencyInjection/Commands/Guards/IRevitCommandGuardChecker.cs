﻿using System;
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
        private readonly Dictionary<Type, List<Predicate<ICommandInfo>>> commandConditions;

        public RevitCommandGuardChecker()
        {
            commandConditions = new Dictionary<Type, List<Predicate<ICommandInfo>>>();
        }

        public bool CanExecute(ICommandInfo commandInfo)
        {
            var commandType = commandInfo.GetCommandType();
            var container = commandInfo.GetContainer();
            var commandData = commandInfo.GetCommandData();

            // Loop through all RevitCommandGuardAttributes to see if we can run the command
            var guardAttrType = typeof(CommandGuardAttribute);
            var attributes = commandType.GetCustomAttributes().Where(a => a.GetType() == guardAttrType);
            if (attributes.Any())
            {
                var methodInfo = guardAttrType.GetMethod(nameof(CommandGuardAttribute.GetCommandGuardType));
                foreach (var attribute in attributes)
                {
                    var guardType = methodInfo.Invoke(attribute, null) as Type;

                    if (guardType != null)
                    {
                        var guard = Activator.CreateInstance(guardType) as IRevitCommandGuard;
                        if (guard != null)
                        {
                            if (!guard.CanExecute(commandInfo))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            var ignoreConditionsType = typeof(IgnoreCommandGuardConditionsAttribute);
            var attributeData = commandType.CustomAttributes;

            // If IgnoreConditions is added to the command, we wont check contions, just allow the command to run
            if (attributeData.Any(a => a.AttributeType == ignoreConditionsType))
            {
                return true;
            }

            // Loop through all conditions to see if we can run the command
            if (commandConditions.ContainsKey(commandType))
            {
                var conditions = commandConditions[commandType];
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

            return true;
        }

        internal void AddCommandTypeCondition(Type commandType, Predicate<ICommandInfo> predicate)
        {
            if (commandConditions.ContainsKey(commandType))
            {
                var commandCondition = commandConditions[commandType];
                commandCondition.Add(predicate);
            }
            else
            {
                commandConditions.Add(commandType, new List<Predicate<ICommandInfo>> { predicate });
            }
        }
    }
}