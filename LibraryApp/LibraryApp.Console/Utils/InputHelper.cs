using System;

namespace LibraryApp.Console.Utils
{ //esta clase hace la conversion en vez de hacerla en program.cs
    //clase utilitaria
    public static class InputHelper
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                System.Console.Write($"{prompt}: ");
                var input = System.Console.ReadLine();
                if (int.TryParse(input, out var value)) return value;
                System.Console.WriteLine("Invalid number. Try again.");
            }
        }

        public static string ReadText(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                System.Console.Write($"{prompt}: ");
                var input = System.Console.ReadLine() ?? "";
                if (allowEmpty || !string.IsNullOrWhiteSpace(input)) return input.Trim();
                System.Console.WriteLine("Value required. Try again.");
            }
        }
    }
}

