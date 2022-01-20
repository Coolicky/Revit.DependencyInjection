using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Revit.DependencyInjection.Unity.RibbonCommands.Attributes;
using Revit.DependencyInjection.Unity.UI;

namespace Revit.DependencyInjection.Unity.RibbonCommands
{
    public static class Extensions
    {
        private static readonly RibbonHelpers RibbonHelpers = new RibbonHelpers(new ImageManager());

        /// <summary>
        /// Automatically adds buttons for Revit Commands decorated with: <see cref="RibbonPushButtonAttribute"/>, <see cref="RibbonSplitButtonAttribute"/> and <see cref="RibbonStackButtonAttribute"/>
        /// </summary>
        /// <param name="ribbonManager"></param>
        /// <param name="config">Configuration options for automatically adding Ribbon Panels and Buttons.</param>
        public static void AddRibbonCommands(this IRibbonManager ribbonManager,
            Action<RibbonCommandConfiguration> config = null)
        {
            var assembly = Assembly.GetCallingAssembly();
            var assemblyName = assembly.GetName().Name;

            var commandItemType = typeof(IRibbonCommandAttribute);

            var ribbonConfig = new RibbonCommandConfiguration
            {
                DefaultPanelName = assemblyName,
                TabName = assemblyName
            };

            config?.Invoke(ribbonConfig);

            var types = assembly.GetTypes();

            var ribbonCommands = types
                .Where(t => t.GetInterfaces().Contains(typeof(IExternalCommand)))
                .Where(t => t.GetCustomAttributes().Any(a => a.GetType().GetInterfaces().Contains(commandItemType)));

            var ribbonCommandDataList = BuildCommandDataStructure(ribbonManager, ribbonCommands, ribbonConfig);
            CreateCommandButtons(ribbonCommandDataList);
        }

        private static void CreateCommandButtons(List<RibbonCommandData> ribbonCommandDataList)
        {
            var addedSplitButtonGroups = new List<string>();
            var addedStackButtonGroups = new List<string>();

            foreach (var commandData in ribbonCommandDataList)
            {
                if (commandData.CommandAttribute is RibbonPushButtonAttribute)
                    ProcessPushButton(commandData);
                else if (commandData.CommandAttribute is RibbonSplitButtonAttribute)
                    ProcessSplitButton(commandData, addedSplitButtonGroups, ribbonCommandDataList);
                else if (commandData.CommandAttribute is RibbonStackButtonAttribute)
                    ProcessStackButton(commandData, addedStackButtonGroups, ribbonCommandDataList);
            }
        }

        private static void ProcessPushButton(RibbonCommandData commandData)
        {
            try
            {
                commandData.RibbonPanel.AddItem(commandData.Button);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void ProcessPushButton(RibbonCommandData commandData, PulldownButton splitGroup)
        {
            try
            {
                splitGroup?.AddPushButton(commandData.Button);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void ProcessSplitButton(RibbonCommandData commandData,
            ICollection<string> addedSplitButtonGroups,
            IEnumerable<RibbonCommandData> ribbonCommandDataList)
        {
            if (!(commandData.CommandAttribute is RibbonSplitButtonAttribute splitButtonAttr) ||
                addedSplitButtonGroups.Contains(splitButtonAttr.SplitButtonGroup)) return;
            addedSplitButtonGroups.Add(splitButtonAttr.SplitButtonGroup);

            // Ignore buttons with not group name
            if (string.IsNullOrWhiteSpace(splitButtonAttr.SplitButtonGroup)) return;

            var items = ribbonCommandDataList
                .Where(d => d.CommandAttribute.PanelName == commandData.CommandAttribute.PanelName)
                .Where(d => d.CommandAttribute is RibbonSplitButtonAttribute)
                .Where(d => ((RibbonSplitButtonAttribute)d.CommandAttribute)?.SplitButtonGroup ==
                            splitButtonAttr.SplitButtonGroup)
                .OrderByDescending(d => ((RibbonSplitButtonAttribute)d.CommandAttribute).SplitGroupPriority)
                .ToList();

            var splitGroup =
                commandData.RibbonPanel.AddItem(new SplitButtonData(splitButtonAttr.SplitButtonGroup,
                    splitButtonAttr.SplitButtonGroup)) as SplitButton;

            items.ForEach(r => ProcessPushButton(r, splitGroup));
        }

        private static void ProcessStackButton(RibbonCommandData commandData,
            ICollection<string> addedStackButtonGroups,
            IEnumerable<RibbonCommandData> ribbonCommandDataList)
        {
            if (!(commandData.CommandAttribute is RibbonStackButtonAttribute stackButtonAttr) ||
                addedStackButtonGroups.Contains(stackButtonAttr.StackButtonGroup)) return;
            addedStackButtonGroups.Add(stackButtonAttr.StackButtonGroup);

            // Ignore buttons with not group name
            if (string.IsNullOrWhiteSpace(stackButtonAttr.StackButtonGroup)) return;

            var items = ribbonCommandDataList
                .Where(d => d.CommandAttribute.PanelName == commandData.CommandAttribute.PanelName)
                .Where(d => d.CommandAttribute is RibbonStackButtonAttribute)
                .Where(d => ((RibbonStackButtonAttribute)d.CommandAttribute)?.StackButtonGroup ==
                            stackButtonAttr.StackButtonGroup)
                .OrderByDescending(d => ((RibbonStackButtonAttribute)d.CommandAttribute).StackGroupPriority)
                .ToList();
            
            AddStackButtonItems(commandData, items);
        }

        private static void AddStackButtonItems(RibbonCommandData commandData, IReadOnlyList<RibbonCommandData> items)
        {
            if (items.Count >= 3)
            {
                try
                {
                    var item0 = items[0];
                    var item1 = items[1];
                    var item2 = items[2];
                    commandData.RibbonPanel.AddStackedItems(item0.Button, item1.Button, item2.Button);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else if (items.Count == 2)
            {
                try
                {
                    var item0 = items[0];
                    var item1 = items[1];
                    commandData.RibbonPanel.AddStackedItems(item0.Button, item1.Button);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static List<RibbonCommandData> BuildCommandDataStructure(IRibbonManager ribbonManager,
            IEnumerable<Type> ribbonCommands, RibbonCommandConfiguration ribbonConfig)
        {
            var commandAttrType = typeof(IRibbonCommandAttribute);
            var ribbonCommandDataList = new List<RibbonCommandData>();

            ribbonManager.CreateTab(ribbonConfig.TabName);
            var panelsToCreate = ribbonConfig.Panels.Distinct();
            foreach (var panelToCreate in panelsToCreate)
            {
                ribbonManager.CreatePanel(ribbonConfig.TabName, panelToCreate);
            }

            var panels = ribbonManager.GetApp().GetRibbonPanels(ribbonConfig.TabName);
            var buttonDataList = new List<PushButtonData>();

            foreach (var commandType in ribbonCommands)
            {
                foreach (var attr in commandType.GetCustomAttributes()
                             .Where(a => a.GetType().GetInterfaces().Contains(commandAttrType)))
                {
                    var commandAttr = attr as IRibbonCommandAttribute;
                    var panel = CreatePanelIfNeeded(commandAttr, ribbonManager, ribbonConfig, panels);
                    var buttonName = GetPushButtonName(commandAttr, ribbonManager, commandType);

                    // Check if a previous item was already created, this would avoid items with same name being created twice. Revit would throw exception.
                    var buttonData = buttonDataList.FirstOrDefault(b => b.Name == buttonName);
                    if (buttonData == null)
                    {
                        buttonData = CreatePushButtonData(commandAttr, buttonName, commandType);
                        buttonDataList.Add(buttonData);
                    }

                    ribbonCommandDataList.Add(new RibbonCommandData
                    {
                        Button = buttonData,
                        CommandAttribute = commandAttr,
                        RibbonPanel = panel
                    });
                }
            }

            ribbonCommandDataList =
                ribbonCommandDataList.OrderByDescending(c => c.CommandAttribute.PanelPriority).ToList();
            return ribbonCommandDataList;
        }

        private static RibbonPanel CreatePanelIfNeeded(IRibbonCommandAttribute commandAttr,
            IRibbonManager ribbonManager, RibbonCommandConfiguration ribbonConfig, List<RibbonPanel> panels)
        {
            var panelName = !string.IsNullOrWhiteSpace(commandAttr.PanelName) ? commandAttr.PanelName : ribbonConfig.DefaultPanelName;
            var panel = panels.FirstOrDefault(p => p.Name == panelName);
            if (panel != null) return panel;
            var panelManager = ribbonManager.CreatePanel(ribbonConfig.TabName, panelName);
            panel = panelManager.GetPanel();
            panels.Add(panel);

            return panel;
        }

        private static string GetPushButtonName(IRibbonCommandAttribute commandAttr, IRibbonManager ribbonManager,
            Type commandType)
        {
            var br = ribbonManager.GetLineBreak();

            string buttonName;
            if (!string.IsNullOrWhiteSpace(commandAttr.FirstLine))
            {
                buttonName = commandAttr.FirstLine;
                if (!string.IsNullOrWhiteSpace(commandAttr.SecondLine))
                {
                    buttonName += br + commandAttr.SecondLine;
                }
            }
            else
            {
                buttonName = commandType.Name.Replace("Command", "");
            }

            return buttonName;
        }

        private static PushButtonData CreatePushButtonData(IRibbonCommandAttribute commandAttr, string buttonName,
            Type commandType)
        {
            try
            {
                var buttonData = RibbonHelpers.CreatePushButtonData(buttonName, commandAttr.Image, commandType,
                    commandAttr.Tooltip, commandAttr.Description);
                if (commandAttr.Availability != null)
                {
                    buttonData.AvailabilityClassName = commandAttr.Availability.FullName;
                }

                return buttonData;
            }
            catch
            {
                return null;
            }
        }
    }
}