namespace Revit.DependencyInjection.Unity.UI
{
    /// <summary>
    /// The resulting of adding a new tab in Revit
    /// </summary>
    public interface IRibbonTabManager
    {
        /// <summary>
        /// Returns the tab name
        /// </summary>
        string GetTabName();
    }
}