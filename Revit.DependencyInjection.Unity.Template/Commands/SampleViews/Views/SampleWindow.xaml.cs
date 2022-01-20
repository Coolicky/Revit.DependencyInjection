using System.Windows;
using Revit.DependencyInjection.Unity.Template.Commands.SampleViews.ViewModels;

namespace Revit.DependencyInjection.Unity.Template.Commands.SampleViews.Views
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