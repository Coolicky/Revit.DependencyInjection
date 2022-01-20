namespace Revit.DependencyInjection.Async
{
    /// <summary>
    /// Data class to hold preferences for Revit async tasks
    /// </summary>
    public class RevitAsyncSettings
    {
        /// <summary>
        /// The name of the handler for Revit async tasks
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the handler will be journable
        /// </summary>
        public bool IsJournalable { get; set; }
    }
}