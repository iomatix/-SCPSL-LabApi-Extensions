using System;
using System.IO;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent utility extensions for filesystem interactions, 
    /// directory migrations, and OS-level platform attribute audits.
    /// </summary>
    internal static class IoExtensions
    {
        /// <summary>
        /// Evaluates defensively whether the targeted path configuration behaves structurally 
        /// as an OS reparse point (Virtual Junction, Symlink, or Hardlink partition).
        /// </summary>
        /// <param name="path">The target filesystem path literal to analyze.</param>
        /// <returns><c>true</c> if the path exists and contains the active reparse point attribute flag; otherwise, <c>false</c>.</returns>
        public static bool IsReparsePoint(this string path)
        {
            if (string.IsNullOrEmpty(path) || (!Directory.Exists(path) && !File.Exists(path)))
            {
                return false;
            }

            try
            {
                return (File.GetAttributes(path) & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
            }
            catch
            {
                return false; // Suppress IO exceptions during background file audits gracefully
            }
        }

        /// <summary>
        /// Iterates over the source directory to discover and mirror file assets into a destination folder boundary 
        /// utilizing a concrete structural search query pattern filter.
        /// </summary>
        /// <param name="sourceDirectory">The originating root folder path anchoring the migration layout.</param>
        /// <param name="destinationDirectory">The targeted destination folder path layout receiving the mirrored file stream.</param>
        /// <param name="searchPattern">The search criteria pattern filtering file collection lookups (e.g. "*.yml", "*.json"). Defaults to "*.*".</param>
        /// <param name="overwrite">If set to <c>true</c>, forcibly overwrites pre-existing assets discovered inside the destination cluster.</param>
        public static void CopyFilesTo(this string sourceDirectory, string destinationDirectory, string searchPattern = "*.*", bool overwrite = true)
        {
            if (string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(destinationDirectory)) return;
            if (!Directory.Exists(sourceDirectory)) return;

            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            try
            {
                string[] discoveredFiles = Directory.GetFiles(sourceDirectory, searchPattern);
                int count = discoveredFiles.Length;

                for (int i = 0; i < count; i++)
                {
                    string sourceFilePath = discoveredFiles[i];
                    string fileName = Path.GetFileName(sourceFilePath);
                    string destinationFilePath = Path.Combine(destinationDirectory, fileName);

                    if (overwrite || !File.Exists(destinationFilePath))
                    {
                        File.Copy(sourceFilePath, destinationFilePath, overwrite);
                    }
                }
            }
            catch (Exception ex)
            {
                iLogger.Error("IoExtensions.CopyFilesTo", $"Bulk file migration collapsed under pattern [{searchPattern}]: {ex.Message}");
            }
        }
    }
}