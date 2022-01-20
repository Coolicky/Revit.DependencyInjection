using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Unity.UI
{
    public class RibbonHelpers
    {
        private readonly ImageManager _imageManager;

        public RibbonHelpers(ImageManager imageManager)
        {
            _imageManager = imageManager;
        }

        public PushButton CreatePushButton(RibbonPanel targetPanel, string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescription = "")
        {
            var currentDll = commandType.Assembly.Location;
            var fullname = commandType.FullName;

            var currentBtn = targetPanel.AddItem(new PushButtonData(targetName, targetName, currentDll, fullname)) as PushButton;
            try
            {
                var currentImage32 = _imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                if (currentBtn != null) currentBtn.LargeImage = currentImage32;
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var currentImage16 = _imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                if (currentBtn != null) currentBtn.Image = currentImage16;
            }
            catch (Exception)
            {
                // ignored
            }

            if (currentBtn == null) return null;
            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescription;
            return currentBtn;
        }

        public ToggleButton CreateToggleButton(RibbonPanel targetPanel, string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescription = "")
        {
            var currentDll = commandType.Assembly.Location;
            var fullname = commandType.FullName;

            var currentBtn = targetPanel.AddItem(new ToggleButtonData(targetName, targetName, currentDll, fullname)) as ToggleButton;
            try
            {
                var currentImage32 = _imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                if (currentBtn != null) currentBtn.LargeImage = currentImage32;
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                var currentImage16 = _imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                if (currentBtn != null) currentBtn.Image = currentImage16;
            }
            catch (Exception)
            {
                // ignored
            }

            if (currentBtn == null) return null;
            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescription;
            return currentBtn;
        }

        public PushButtonData CreatePushButtonData(string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescription = "")
        {
            var currentDll = commandType.Assembly.Location;
            string fullname = commandType.FullName;

            PushButtonData currentBtn = new PushButtonData(targetName, targetName, currentDll, fullname);
            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage32 = _imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                currentBtn.LargeImage = currentImage32;
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage16 = _imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                currentBtn.Image = currentImage16;
            }
            catch (Exception)
            {
                // ignored
            }

            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescription;

            return currentBtn;
        }

        public SplitButton CreateSplitButton(RibbonPanel targetPanel, IList<PushButtonData> targetPushButtons)
        {
            if (targetPushButtons.Count <= 0) return null;
            var targetName = targetPushButtons.FirstOrDefault()?.Name;
            var currentSplitButton = targetPanel.AddItem(new SplitButtonData(targetName, targetName)) as SplitButton;
            foreach (var currentPushButton in targetPushButtons)
            {
                currentSplitButton?.AddPushButton(currentPushButton);
            }

            return currentSplitButton;
        }

    }
}