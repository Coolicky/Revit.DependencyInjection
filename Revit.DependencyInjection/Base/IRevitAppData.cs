namespace Revit.DependencyInjection.Base
{
    public interface IRevitAppData
    {
        /// <summary>
        /// Revit's build version
        /// </summary>
        string GetVersionBuild();

        /// <summary>
        /// Revit's version name
        /// </summary>
        string GetVersionName();

        /// <summary>
        /// Revit's version number
        /// </summary>
        string GetVersionNumber();

        /// <summary>
        /// Revit's build version name
        /// </summary>
        string GetSubVersionNumber();
    }
}