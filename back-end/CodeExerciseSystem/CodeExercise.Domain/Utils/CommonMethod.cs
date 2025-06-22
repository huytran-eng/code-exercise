namespace CodeExercise.Core.Utils
{
    public class CommonMethod
    {
        public static string GenerateSlug(string name)
        {
            return name
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace(",", "")
                    .Replace(".", "")
                    .Replace("?", "")
                    .Replace("'", "")
                    .Replace("\"", "")
                    .Replace(":", "")
                    .Replace(";", "")
                    .Replace("/", "")
                    .Replace("\\", "")
                    .Replace("&", "and")
                    .Replace("--", "-")
                    .Trim('-');
        }
    }
}
