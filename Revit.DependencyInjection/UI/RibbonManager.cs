using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.UI
{
    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonManager : IRibbonManager
    {
        private readonly UIControlledApplication app;
        private readonly ImageManager imageManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public RibbonManager(UIControlledApplication app, ImageManager imageManager)
        {
            this.app = app;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Creates a Ribbon Tab
        /// </summary>
        public IRibbonTabManager CreateTab(string name)
        {
            try
            {
                this.app.CreateRibbonTab(name);
            }
            catch
            {
            }

            return new RibbonTabManager(this.app, name, this.imageManager);
        }

        /// <summary>
        /// Creates a Ribbon Panel on Revit's Addins Tab
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = this.app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(null, panel, this.imageManager);
            return itembuilder;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string tabName, string panelName)
        {
            try
            {
                this.app.CreateRibbonTab(tabName);
            }
            catch
            {
            }

            var panel = this.app.CreateRibbonPanel(tabName, panelName);
            var itembuilder = new RibbonPanelManager(tabName, panel, this.imageManager);
            return itembuilder;
        }

        /// <summary>
        /// Get Revit UI Controlled App
        /// </summary>
        /// <returns></returns>
        public UIControlledApplication GetApp()
        {
            return this.app;
        }

        /// <summary>
        /// Gets Ribbon line break string
        /// </summary>
        public string GetLineBreak()
        {
            return "\r\n";
        }
    }
}