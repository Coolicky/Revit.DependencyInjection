using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.DependencyInjection.Base
{
    public interface IRevitContext
    {
        /// <summary>
        /// Gets the current Revit Application
        /// </summary>
        /// <returns></returns>
        Application GetApplication();
        /// <summary>
        /// Gets the current Revit Document
        /// </summary>
        /// <returns></returns>
        Document GetDocument();
        /// <summary>
        /// Gets the current Revit UI Application
        /// </summary>
        /// <returns></returns>
        UIApplication GetUIApplication();
        /// <summary>
        /// Gets the current Revit UIDocument
        /// </summary>
        /// <returns></returns>
        UIDocument GetUIDocument();
        /// <summary>
        /// Identifies if Revit is in the current context (Revit API context)
        /// </summary>
        bool IsInRevitContext();
        
    }
}