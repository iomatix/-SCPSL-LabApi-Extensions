using LabApi.Extensions.Misc;
using System;
using System.IO;

namespace LabApi.Extensions
{
    /// <summary>
    /// Simple helpers for filesystem checks and file copying.
    /// </summary>
    public static class IoExtensions
    {
        /// <summary>
        /// Returns true if the path exists and is a reparse point (symlink, junction, etc.).
        /// </summary>
        public static bool IsReparsePoint(this string path)
        {
            if (string.IsNullOrEmpty(path) ||
                (!Directory.Exists(path) && !File.Exists(path)))
            {
                return false;
            }

            try
            {
                return (File.GetAttributes(path) & FileAttributes.ReparsePoint) == FileAttributes.ReparsePoint;
            }
            catch
            {
                // Ignore IO exceptions (locked files, missing permissions, etc.)
                return false;
            }
        }

        /// <summary>
        /// Copies files from one directory to another using a search pattern.
        /// Creates the destination directory if needed.
        /// </summary>
        /// <param name="sourceDirectory">Folder to copy files from.</param>
        /// <param name="destinationDirectory">Folder to copy files into.</param>
        /// <param name="searchPattern">File filter (e.g. "*.json"). Defaults to "*.*".</param>
        /// <param name="overwrite">If true, replaces existing files.</param>
        public static void CopyFilesTo(
            this string sourceDirectory,
            string destinationDirectory,
            string searchPattern = "*.*",
            bool overwrite = true)
        {
            if (string.IsNullOrEmpty(sourceDirectory) ||
                string.IsNullOrEmpty(destinationDirectory))
                return;

            if (!Directory.Exists(sourceDirectory))
                return;

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            try
            {
                var files = Directory.GetFiles(sourceDirectory, searchPattern);
                int count = files.Length;

                for (int i = 0; i < count; i++)
                {
                    string src = files[i];
                    string name = Path.GetFileName(src);
                    string dst = Path.Combine(destinationDirectory, name);

                    if (overwrite || !File.Exists(dst))
                        File.Copy(src, dst, overwrite);
                }
            }
            catch (Exception ex)
            {
                iLogger.Error("IoExtensions.CopyFilesTo",
                    $"File copy failed for pattern '{searchPattern}': {ex.Message}");
            }
        }
    }
}
