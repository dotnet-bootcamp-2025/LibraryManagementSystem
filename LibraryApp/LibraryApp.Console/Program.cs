// See https://aka.ms/new-console-template for more information

using LibraryApp.Console.Domain;
using System;


public class Program
{
    private static readonly List<LibraryItem> _items = new();
    public static void Main()
    {
        Console.WriteLine("Library App!");
        Seed();
        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            int choice;
            if (!int.TryParse(input, out choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }
            Console.WriteLine();
            switch (choice)
            {
                case 1: ListItems(); break;
                //case 2: SearchItems(); break;
                case 3: AddBook(); break;
                case 4: AddMagazine(); break;
                case 0: exit = true; break;
                default: Console.WriteLine("Unknown option."); break;
            }
            if (!exit)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        Console.WriteLine("Goodbye!");
    }
    static void ShowMenu()
    {
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1) List all items");
        Console.WriteLine("2) Search items by title (TBD)");
        Console.WriteLine("3) Add Book");
        Console.WriteLine("4) Add Magazine");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }
    static void ListItems()
    {
        if (_items.Count == 0) { Console.WriteLine("No items."); return; }
        Console.WriteLine("Items:");
        foreach (var item in _items)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }
    // Seed fake data for the demo
    static void Seed()
    {
        _items.Add(new Book(1, "Clean Code", "Robert C. Martin", 464));
        _items.Add(new Book(2, "The Pragmatic Programmer", "Andrew Hunt", 352));
        _items.Add(new Magazine(3, "DotNET Weekly", 120, "DevPub"));
        _items.Add(new Magazine(4, "Tech Monthly", 58, "TechPress"));
    }
    static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");
        var _nextItemId = _items.Count > 0 ? _items.Max(i => i.Id) : 0;
        var book = new Book(_nextItemId++, title, author, pages);
        _items.Add(book);
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }
    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");
        var _nextItemId = _items.Count > 0 ? _items.Max(i => i.Id) : 0;
        var mag = new Magazine(_nextItemId++, title, issue, publisher);
        _items.Add(mag);
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }
}

