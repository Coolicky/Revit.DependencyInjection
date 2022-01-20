using System;

namespace Revit.DependencyInjection.Unity.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IgnoreCommandGuardConditionsAttribute : Attribute
    {
    }
}