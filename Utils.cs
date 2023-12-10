namespace HelloWorld
{
    public class Utils
    {
        public static string EscapeUnwantedChars(string? input)
        {
            if (input != null)
            {
                string output = input.Replace("'", "''");
                return output;
            }
            return "null";
        }
    }
}