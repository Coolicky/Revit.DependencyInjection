using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.UI
{
    public interface IRibbonManager
    {
        /// <summary>
        /// Get Revit UI Controlled App
        /// </summary>
        /// <returns></returns>
        UIControlledApplication GetApp();
        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        IRibbonPanelManager CreatePanel(string tabName, string panelName);
        /// <summary>
        /// Creates a Ribbon Panel on Revit's Addins Tab
        /// </summary>
        IRibbonPanelManager CreatePanel(string panelName);
        /// <summary>
        /// Creates a Ribbon Tab
        /// </summary>
        IRibbonTabManager CreateTab(string name);
        /// <summary>
        /// Gets Ribbon line break string
        /// </summary>
        string GetLineBreak();
    }
}