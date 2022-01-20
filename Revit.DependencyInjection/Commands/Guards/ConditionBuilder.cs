using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Revit.DependencyInjection.Commands.Guards
{
    internal class ConditionBuilder : IConditionBuilder, IConditionBuilderProvider
    {
        private readonly List<Type> _addedCommands;
        private readonly List<Type> _removedCommands;

        private Predicate<ICommandInfo> _predicate;

        private readonly List<Func<Type, bool>> _filters;

        public ConditionBuilder()
        {
            _addedCommands = new List<Type>();
            _removedCommands = new List<Type>();
            _filters = new List<Func<Type, bool>>();
        }

        public void CanExecute(Predicate<ICommandInfo> predicate)
        {
            _predicate = predicate;
        }

        public IConditionBuilder ExceptCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand
        {
            var type = typeof(TCommand);
            _removedCommands.Add(type);
            return this;
        }
        public IConditionBuilder WhereCommandType(Func<Type, bool> commandTypeFilter)
        {
            _filters.Add(commandTypeFilter);
            return this;
        }


        public IConditionBuilder ForCommand<TCommand>() where TCommand : ICanBeGuardedRevitCommand
        {
            var type = typeof(TCommand);
            _addedCommands.Add(type);
            return this;
        }

        public IConditionBuilder ForCommandsInAssembly(Assembly assembly)
        {
            var interFaceType = typeof(ICanBeGuardedRevitCommand);
            var types = assembly.GetTypes().Where(t => t.GetInterfaces().FirstOrDefault(i => i == interFaceType) != null);
            _addedCommands.AddRange(types);
            return this;
        }

        public Predicate<ICommandInfo> GetPredicate()
        {
            return _predicate;
        }

        public List<Type> GetCommandTypes()
        {
            var commands = _addedCommands.Distinct().Except(_removedCommands.Distinct());

            foreach (var filter in _filters)
            {
                commands = commands.Where(filter);
            }

            return commands.ToList();
        }


    }
}