namespace Revit.DependencyInjection.Unity.Commands.Guards
{
    public interface IRevitCommandGuard
    {
        bool CanExecute(ICommandInfo commandInfo);
    }
}