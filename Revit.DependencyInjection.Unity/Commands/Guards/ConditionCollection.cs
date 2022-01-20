using System.Collections.Generic;

namespace Revit.DependencyInjection.Unity.Commands.Guards
{
    internal class ConditionCollection : IConditionCollection
    {
        readonly List<IConditionBuilderProvider> _builders;

        public ConditionCollection()
        {
            _builders = new List<IConditionBuilderProvider>();
        }

        public IConditionBuilder AddCondition()
        {
            var conditionBuilder = new ConditionBuilder();
            _builders.Add(conditionBuilder);
            return conditionBuilder;
        }

        internal List<IConditionBuilderProvider> GetBuilders()
        {
            return _builders;
        }
    }
}