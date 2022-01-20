using System;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Base
{
internal class RevitContext : IRevitContext, IRevitContextProvider
    {
        private Document _document;
        private Application _application;
        private UIDocument _uiDocument;
        private UIApplication _uiApplication;

        private bool _hookupViewChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public RevitContext()
        {
        }

        /// <summary>
        /// Gets the current Revit Document
        /// </summary>
        /// <returns></returns>
        public Document GetDocument()
        {
            return _document;
        }

        /// <summary>
        /// Gets the current Revit UIDocument
        /// </summary>
        /// <returns></returns>
        public UIDocument GetUIDocument()
        {
            return _uiDocument;
        }

        /// <summary>
        /// Gets the current Revit Application
        /// </summary>
        /// <returns></returns>
        public Application GetApplication()
        {
            return _application;
        }

        /// <summary>
        /// Gets the current Revit UI Application
        /// </summary>
        /// <returns></returns>
        public UIApplication GetUIApplication()
        {
            return _uiApplication;
        }

        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        public void HookupRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated += OnDocumentCreated;
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            application.ControlledApplication.DocumentOpened += OnDocumentOpened;
            application.ControlledApplication.DocumentClosed += OnDocumentClosed;
        }

        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        public void UnhookRevitEvents(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentCreated -= OnDocumentCreated;
            application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
            application.ControlledApplication.DocumentOpened -= OnDocumentOpened;
            application.ControlledApplication.DocumentClosed -= OnDocumentClosed;

            // Makes sure to unhook the ViewChanged event and get rid of reference to uiApplication
            UnhookViewChanged();
        }

        /// <summary>
        /// Hooks up Revit Events to the context
        /// </summary>
        public void HookupRevitEvents(UIApplication application)
        {
            if (application?.ActiveUIDocument is UIDocument uIDocument)
            {
                UpdateContext(uIDocument.Document);
            }

            if (application == null) return;
            application.Application.DocumentCreated += OnDocumentCreated;
            application.Application.DocumentChanged += OnDocumentChanged;
            application.Application.DocumentOpened += OnDocumentOpened;
            application.Application.DocumentClosed += OnDocumentClosed;
        }

        /// <summary>
        /// Unhooks Revit Events to the context
        /// </summary>
        public void UnhookRevitEvents(UIApplication application)
        {
            if (application == null) return;
            application.Application.DocumentCreated -= OnDocumentCreated;
            application.Application.DocumentChanged -= OnDocumentChanged;
            application.Application.DocumentOpened -= OnDocumentOpened;
            application.Application.DocumentClosed -= OnDocumentClosed;

            // Makes sure to unhook the ViewChanged event and get rid of reference to uiApplication
            UnhookViewChanged();
        }

        private void OnDocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            var doc = e.Document;
            UpdateContext(doc);
            HookUpViewChanged(doc);
        }

        private void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var doc = e.GetDocument();
            UpdateContext(doc);
            HookUpViewChanged(doc);
        }

        private void OnDocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            var doc = e.Document;
            UpdateContext(doc);
            HookUpViewChanged(doc);
        }

        private void OnViewChanged(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            var doc = e.Document;
            UpdateContext(doc);
        }

        private void OnDocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
            // If the ActiveUIDocument is not null, it means that there is another document in the background
            // The DocumentChanged or ViewChanged event will pickup that new Document
            // So in the case of null ActiveUIDocument we cleanup the container from Documents and Applications
            if (_uiApplication?.ActiveUIDocument == null)
            {
                UpdateContext(null);
            }
        }

        private void UpdateContext(Document doc)
        {
            _document = doc;

            // In the case of a valid Document context
            if (doc != null)
            {
                var uiDoc = new UIDocument(doc);

                _application = doc.Application;
                _uiDocument = uiDoc;
                _uiApplication = uiDoc.Application;
            }
            else
            {
                _application = null;
                _uiDocument = null;
                _uiApplication = null;
            }

        }

        private void HookUpViewChanged(Document doc)
        {
            if (_hookupViewChanged) return;
            _hookupViewChanged = true;
            var uiDoc = new UIDocument(doc);
            uiDoc.Application.ViewActivated += OnViewChanged;
        }

        private void UnhookViewChanged()
        {
            if (!_hookupViewChanged || _uiApplication == null) return;
            _hookupViewChanged = false;
            _uiApplication.ViewActivated -= OnViewChanged;
        }

        /// <summary>
        /// Identifies if Revit is in the current context (Revit API context)
        /// </summary>
        public bool IsInRevitContext()
        {
            if (_document == null)
            {
                return false;
            }

            if (_document.IsModifiable)
            {
                return true;
            }

            using (var t = new Transaction(_document, "Context"))
            {
                try
                {
                    t.Start();
                    t.RollBack();
                    return true;
                }
                catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}