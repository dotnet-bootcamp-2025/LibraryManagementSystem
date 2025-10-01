using static System.Console;

namespace LibraryApp.Console.Utils
{
    public static class InputHelper
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Write($"{prompt}: ");
                var input = ReadLine();
                if (int.TryParse(input, out var value)) return value;
                WriteLine("Número inválido. Intenta de nuevo.");
            }
        }

        public static string ReadText(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                Write($"{prompt}: ");
                var input = ReadLine() ?? "";
                if (allowEmpty || !string.IsNullOrWhiteSpace(input)) return input.Trim();
                WriteLine("Se requiere un valor. Intenta de nuevo.");
            }
        }
    }
}
