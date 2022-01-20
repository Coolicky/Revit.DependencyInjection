using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Unity.UI
{
    /// <summary>
    /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
    /// </summary>
    public class RibbonTabManager : IRibbonTabManager
    {
        private readonly UIControlledApplication _app;
        private readonly string _tabName;
        private readonly ImageManager _imageManager;

        /// <summary>
        /// Ribbon Manager will help you create Tabs, Panels and Buttons in Revit
        /// </summary>
        public RibbonTabManager(UIControlledApplication app, string tabName, ImageManager imageManager)
        {
            _app = app;
            _tabName = tabName;
            _imageManager = imageManager;
        }

        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        public string GetTabName()
        {
            return _tabName;
        }

        /// <summary>
        /// Creates a Ribbon Panel
        /// </summary>
        public IRibbonPanelManager CreatePanel(string panelName)
        {
            var panel = _app.CreateRibbonPanel(panelName);
            var itembuilder = new RibbonPanelManager(_tabName, panel, _imageManager);
            return itembuilder;
        }
    }
}