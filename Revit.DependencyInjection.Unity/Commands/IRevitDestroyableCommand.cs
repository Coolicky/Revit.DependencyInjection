using Unity;

namespace Revit.DependencyInjection.Unity.Commands
{
    /// <summary>
    /// Holds lifecycle hook for Commands to be able to hook up to disposing container
    /// </summary>
    public interface IRevitDestroyableCommand
    {
        /// <summary>
        /// External Command lifecycle hook which is called just before the container is disposed.
        /// </summary>
        void OnDestroy(IUnityContainer container);
    }
}