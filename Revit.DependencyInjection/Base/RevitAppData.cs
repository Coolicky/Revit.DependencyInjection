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
    public class RevitAppData : IRevitAppData
    {
        internal string subVersionNumber;
        internal string versionNumber;
        internal string versionBuild;
        internal string versionName;

        /// <summary>
        /// Revit's sub version number
        /// </summary>
        public string GetSubVersionNumber()
        {
            return subVersionNumber;
        }

        /// <summary>
        /// Revit's build version
        /// </summary>
        public string GetVersionBuild()
        {
            return versionBuild;
        }

        /// <summary>
        /// Revit's build version name
        /// </summary>
        public string GetVersionName()
        {
            return versionName;
        }

        /// <summary>
        /// Revit's version number
        /// </summary>
        public string GetVersionNumber()
        {
            return versionNumber;
        }
    }
}