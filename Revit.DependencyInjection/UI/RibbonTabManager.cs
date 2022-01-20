using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.UI
{
    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonTabManager : IRibbonTabManager
    {
        private readonly UIControlledApplication app;
        private readonly string tabName;
        private readonly ImageManager imageManager;

        /// <summary>
        /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
        /// </summary>
        public RibbonTabManager(UIControlledApplication app, string tabName, ImageManager imageManager)
        {
            this.app = app;
            this.tabName = tabName;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        public string GetTabName()
        {
            return this.tabName;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(this.tabName, panel, this.imageManager);
            return itembuilder;
        }
    }
}