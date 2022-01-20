using System.Windows;
using Revit.SampleCommands.Commands.SampleViews.ViewModels;

namespace Revit.SampleCommands.Commands.SampleViews.Views
{
    public partial class SampleWindow : Window
    {
        public SampleWindowViewModel ViewModel { get; }

        public SampleWindow(SampleWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}