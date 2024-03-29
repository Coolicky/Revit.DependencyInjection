﻿using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Unity.UI
{
    /// <summary>
    /// The responsible for adding UI Elements in the Ribbon, e.g. Buttons and Separators.
    /// </summary>
    public class RibbonPanelManager : IRibbonPanelManager
    {
        private readonly string _tabName;
        private readonly RibbonPanel _panel;
        private readonly RibbonHelpers _ribbonHelpers;

        /// <summary>
        /// The responsible for adding UI Elements in the Ribbon, e.g. Buttons and Separators.
        /// </summary>
        public RibbonPanelManager(string tabName, RibbonPanel panel, ImageManager imageManager)
        {
            _tabName = tabName;
            _panel = panel;
            _ribbonHelpers = new RibbonHelpers(imageManager);
        }

        /// <summary>
        /// Gets the current tab name.
        /// </summary>
        public string GetTabName()
        {
            return _tabName;
        }

        /// <summary>
        /// Gets the current panel name.
        /// </summary>
        /// <returns></returns>
        public RibbonPanel GetPanel()
        {
            return _panel;
        }

        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        public PushButton AddPushButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return _ribbonHelpers.CreatePushButton(_panel, name, image, typeof(TExternalCommand), tooltip, description);
        }
        
        /// <summary>
        /// Adds a new PushButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButton.</returns>
        public PushButton AddPushButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = _ribbonHelpers.CreatePushButton(_panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Creates a new PushButtonData for a Command with the default CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        public PushButtonData CreatePushButtonData<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            return _ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
        }

        /// <summary>
        /// Creates a new PushButtonData for a Command with a custom CommandAvailability, to be later added to another UI Element.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created PushButtonData.</returns>
        public PushButtonData CreatePushButtonData<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = _ribbonHelpers.CreatePushButtonData(name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with the default CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        public ToggleButton AddToggleButton<TExternalCommand>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new()
        {
            var button = _ribbonHelpers.CreateToggleButton(_panel, name, image, typeof(TExternalCommand), tooltip, description);
            return button;
        }

        /// <summary>
        /// Adds a new ToggleButton for a Command in the Revit UI with a custom CommandAvailability.
        /// </summary>
        /// <typeparam name="TExternalCommand">The type of the External Command.</typeparam>
        /// <typeparam name="TAvailability">The type of the Command Availability.</typeparam>
        /// <param name="name">The name of the Command as it appears in Revit UI.</param>
        /// <param name="image">The button image name, no path. The image should be a resource in the current project.</param>
        /// <param name="tooltip">The tooltip that Revit wil show for this button.</param>
        /// <param name="description">The description that Revit wil show for this button.</param>
        /// <returns>The created ToggleButton.</returns>
        public ToggleButton AddToggleButton<TExternalCommand, TAvailability>(string name, string image = null, string tooltip = null, string description = null) where TExternalCommand : class, IExternalCommand, new() where TAvailability : class, IExternalCommandAvailability, new()
        {
            var button = _ribbonHelpers.CreateToggleButton(_panel, name, image, typeof(TExternalCommand), tooltip, description);
            button.AvailabilityClassName = typeof(TAvailability).FullName;
            return button;
        }

        /// <summary>
        /// Adds a SplitButton in the Revit UI.
        /// </summary>
        /// <param name="pushButtonDataList">The list of PushButtons to add.</param>
        /// <returns>The created SplitButton</returns>
        public SplitButton AddSplitButton(List<PushButtonData> pushButtonDataList)
        {
            return _ribbonHelpers.CreateSplitButton(_panel, pushButtonDataList);
        }

        /// <summary>
        /// Adds a stacked containing 2 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2)
        {
            return _panel.AddStackedItems(item1, item2);
        }

        /// <summary>
        /// Adds a stacked containing 3 UI Elements in the current panel.
        /// </summary>
        /// <param name="item1">Top UI Element.</param>
        /// <param name="item2">Middle UI Element.</param>
        /// <param name="item3">Bottom UI Element.</param>
        /// <returns>The list of elements added</returns>
        public IList<RibbonItem> AddStackedItems(RibbonItemData item1, RibbonItemData item2, RibbonItemData item3)
        {
            return _panel.AddStackedItems(item1, item2, item3);
        }

        /// <summary>
        /// Adds a separator in the current panel.
        /// </summary>
        public void AddSeparator()
        {
            _panel.AddSeparator();
        }

        /// <summary>
        /// Adds a slide out in the current panel.
        /// </summary>
        public void AddSlideOut()
        {
            _panel.AddSlideOut();
        }

    }
}