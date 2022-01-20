using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.UI
{
    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonManager : IRibbonManager
    {
        private readonly UIControlledApplication _app;
        private readonly ImageManager _imageManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public RibbonManager(UIControlledApplication app, ImageManager imageManager)
        {
            _app = app;
            _imageManager = imageManager;
        }

        /// <summary>
        /// Creates a Ribbon Tab
        /// </summary>
        public IRibbonTabManager CreateTab(string name)
        {
            try
            {
                _app.CreateRibbonTab(name);
            }
            catch
            {
                // ignored
            }

            return new RibbonTabManager(_app, name, _imageManager);
        }

        /// <summary>
        /// Creates a Ribbon Panel on Revit's Addins Tab
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = _app.CreateRibbonPanel(panelName);
            var itemBuilder = new RibbonPanelManager(null, panel, _imageManager);
            return itemBuilder;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string tabName, string panelName)
        {
            try
            {
                _app.CreateRibbonTab(tabName);
            }
            catch
            {
                // ignored
            }

            var panel = _app.CreateRibbonPanel(tabName, panelName);
            var itemBuilder = new RibbonPanelManager(tabName, panel, _imageManager);
            return itemBuilder;
        }

        /// <summary>
        /// Get Revit UI Controlled App
        /// </summary>
        /// <returns></returns>
        public UIControlledApplication GetApp()
        {
            return _app;
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