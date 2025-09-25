using LibraryApp.ConsoleApp.Utils;
using LibraryApp.Domain;
using LibraryApp.Services;

namespace LibraryApp.ConsoleApp;

public class Program
{
    private static readonly List<LibraryItem> _items = new();
    private static readonly LibraryService _service = new();

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        _service.Seed();

        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            var choice = InputHelper.ReadInt("Select option");
            Console.WriteLine();

            switch (choice)
            {
                case 1: ListItems(); break;
                case 2: SearchItems(); break;
                case 3: AddBook(); break;
                case 4: AddMagazine(); break;
                case 5: ListMembers(); break;
                case 6: RegisterMember(); break;
                case 7: BorrowItem(); break;
                case 8: ReturnItem(); break;
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

    private static void ShowMenu()
    {
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1) List all items");
        Console.WriteLine("2) Search items by title");
        Console.WriteLine("3) Add Book");
        Console.WriteLine("4) Add Magazine");
        Console.WriteLine("5) List members");
        Console.WriteLine("6) Register member");
        Console.WriteLine("7) Borrow item");
        Console.WriteLine("8) Return item");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }

    private static void ListItems()
    {
        if (_service.Items.Count == 0) { Console.WriteLine("No items."); return; }

        Console.WriteLine("Items:");
        foreach (var item in _service.Items)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    private static void SearchItems()
    {
        var term = InputHelper.ReadText("Search term (leave empty for all)", allowEmpty: true);
        var results = _service.FindItems(term).ToList();

        if (!results.Any()) { Console.WriteLine("No matches."); return; }

        Console.WriteLine("Matches:");
        foreach (var item in results)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    private static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");
        var book = _service.AddBook(title, author, pages);
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }

    private static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");
        var mag = _service.AddMagazine(title, issue, publisher);
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }

    private static void ListMembers()
    {
        if (_service.Members.Count == 0) { Console.WriteLine("No members."); return; }

        Console.WriteLine("Members:");
        foreach (var m in _service.Members)
            Console.WriteLine(m);
    }

    private static void RegisterMember()
    {
        var name = InputHelper.ReadText("Member name");
        var member = _service.RegisterMember(name);
        Console.WriteLine($"Registered: {member}");
    }

    private static void BorrowItem()
    {
        var memberId = InputHelper.ReadInt("Member Id");
        var itemId = InputHelper.ReadInt("Item Id");
        if (_service.BorrowItem(memberId, itemId, out var message))
            Console.WriteLine(message);
        else
            Console.WriteLine($"Could not borrow: {message}");
    }

    private static void ReturnItem()
    {
        var memberId = InputHelper.ReadInt("Member Id");
        var itemId = InputHelper.ReadInt("Item Id");
        if (_service.ReturnItem(memberId, itemId, out var message))
            Console.WriteLine(message);
        else
            Console.WriteLine($"Could not return: {message}");
    }
}
