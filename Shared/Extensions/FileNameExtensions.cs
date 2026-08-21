using System.Text.RegularExpressions;

namespace AI200Labs.Shared.Extensions;

public static class FileNameExtensions
{
    /// <summary>
    /// Sanitizes a filename to be Azure-safe by removing/replacing invalid characters.
    /// </summary>
    public static string SanitizeForAzure(this string fileName)
    {
        // Replace dashes and other problematic characters with underscores
        // Keep only alphanumeric, dots, underscores, and hyphens
        var sanitized = Regex.Replace(fileName, @"[^a-zA-Z0-9._-]", "_");
        
        // Remove consecutive underscores
        sanitized = Regex.Replace(sanitized, "_+", "_");
        
        // Trim underscores from start/end
        sanitized = sanitized.Trim('_');
        
        return sanitized;
    }

    /// <summary>
    /// Extracts filename without extension and sanitizes it.
    /// </summary>
    public static string GetSafeFileNameWithoutExtension(this string filePath)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        return nameWithoutExtension.SanitizeForAzure();
    }
}
