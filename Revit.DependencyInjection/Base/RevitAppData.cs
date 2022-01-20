namespace Revit.DependencyInjection.Base
{
    public class RevitAppData : IRevitAppData
    {
        internal string SubVersionNumber;
        internal string VersionNumber;
        internal string VersionBuild;
        internal string VersionName;

        /// <summary>
        /// Revit's sub version number
        /// </summary>
        public string GetSubVersionNumber()
        {
            return SubVersionNumber;
        }

        /// <summary>
        /// Revit's build version
        /// </summary>
        public string GetVersionBuild()
        {
            return VersionBuild;
        }

        /// <summary>
        /// Revit's build version name
        /// </summary>
        public string GetVersionName()
        {
            return VersionName;
        }

        /// <summary>
        /// Revit's version number
        /// </summary>
        public string GetVersionNumber()
        {
            return VersionNumber;
        }
    }
}