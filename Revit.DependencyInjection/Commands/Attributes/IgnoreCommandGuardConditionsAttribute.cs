using System;

namespace Revit.DependencyInjection.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IgnoreCommandGuardConditionsAttribute : Attribute
    {
    }
}