using LibrartApp.Domain;
using System;


namespace LibraryApp.Console.Utils;

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

    public static void PrintList(IEnumerable<LibraryItem> items)
    {
        var any = false;

        foreach (var item in items)
        {
            any = true;
            var title = item.Title;
            System.Console.WriteLine($"{item.Id} {item.Title} (IsBorrowed: {item.IsBorrowed})");
        }

        if (!any)
        {
            System.Console.WriteLine("No items");
        }
    }
}