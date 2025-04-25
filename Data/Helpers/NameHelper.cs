using System.Linq;

namespace Data.Helpers
{
    public static class NameHelper
    {
        public static string SnakeToPascal(this string snake)
        {
            if (string.IsNullOrWhiteSpace(snake)) return snake;

            var parts = snake.Split('_');
            return string.Concat(parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
        }

        public static string PascalToSnake(this string pascal)
        {
            if (string.IsNullOrWhiteSpace(pascal)) return pascal;

            var result = new System.Text.StringBuilder();
            for (int i = 0; i < pascal.Length; i++)
            {
                char c = pascal[i];
                if (char.IsUpper(c) && i > 0)
                    result.Append('_');
                result.Append(char.ToLower(c));
            }

            return result.ToString();
        }
    }
}
