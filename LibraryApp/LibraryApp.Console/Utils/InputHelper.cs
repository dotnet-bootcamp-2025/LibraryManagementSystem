using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Console.Domain;

namespace LibraryApp.Console.Utils
{ }
    public static class InputHelper
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out var value)) return value;
                Console.WriteLine("Invalid number. Try again.");
            }
        }
        public static string ReadText(string prompt, bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine() ?? "";
                if (allowEmpty || !string.IsNullOrWhiteSpace(input)) return input.Trim();
                Console.WriteLine("Value required. Try again.");
            }
        }
    }
