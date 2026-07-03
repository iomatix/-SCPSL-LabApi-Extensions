namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance string manipulation and network identity normalization layers.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Enforces standard lowercase invariant formatting on a raw network identifier token,
        /// mitigating platform-specific auth casing anomalies and dictionary key mismatches.
        /// </summary>
        /// <param name="userId">The raw user identity string to be processed.</param>
        /// <returns>A sanitized, lowercase invariant string, or an empty string if the source is null.</returns>
        public static string NormalizeUserId(this string userId)
        {
            return userId != null ? userId.ToLowerInvariant() : string.Empty;
        }
    }
}