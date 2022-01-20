using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using Prism.Commands;
using Prism.Mvvm;
using Revit.DependencyInjection.Unity.Template.Interfaces;

namespace Revit.DependencyInjection.Unity.Template.Commands.SampleViews.ViewModels
{
    public class SampleWindowViewModel : BindableBase
    {
        private readonly ISampleSelector _sampleSelector;

        public SampleWindowViewModel(ISampleSelector sampleSelector)
        {
            _sampleSelector = sampleSelector;
            SelectElementsCommand = new DelegateCommand(SelectElements);
        }

        private ObservableCollection<Element> _elements;

        public ObservableCollection<Element> Elements
        {
            get => _elements;
            set => SetProperty(ref _elements, value);
        }

        private Element _selectedElement;

        public Element SelectedElement
        {
            get => _selectedElement;
            set
            {
                SetProperty(ref _selectedElement, value);
                if (_selectedElement != null)
                    _sampleSelector.SelectElement(_selectedElement.Id);
            }
        }

        public DelegateCommand SelectElementsCommand { get; set; }

        private async void SelectElements()
        {
            Elements = new ObservableCollection<Element>(await _sampleSelector.GetSelectedOrSelectElements());
        }
    }
}