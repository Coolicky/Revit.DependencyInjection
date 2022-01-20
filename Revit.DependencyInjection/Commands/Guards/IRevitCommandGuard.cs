namespace Revit.DependencyInjection.Commands.Guards
{
    public interface IRevitCommandGuard
    {
        bool CanExecute(ICommandInfo commandInfo);
    }
}