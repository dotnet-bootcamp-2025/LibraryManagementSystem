// Refactor incluyendo Case 5 y 6

//using LibraryApp.Console.Domain;
//using LibraryApp.Console.Services;
//using LibraryApp.Console.Utils;

using LibraryApp.Services;
using LibraryApp.Console.Utils;
using LibraryApp.Domain;

public class Program
{
    private static readonly ILibraryService _service = new LibraryService();

    public static void Main()
    {
        Console.WriteLine("Library App!");
        _service.Seed(); // inicializa con datos de ejemplo

        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1: ListItems(); break;
                case 2: SearchItems(); break;
                case 3: AddBook(); break;
                case 4: AddMagazine(); break;
                case 5: RegisterMember(); break;
                case 6: ListMembers(); break;
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

    static void ShowMenu()
    {
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1) List all items");
        Console.WriteLine("2) Search items by title");
        Console.WriteLine("3) Add Book");
        Console.WriteLine("4) Add Magazine");
        Console.WriteLine("5) Register Member");
        Console.WriteLine("6) List Members");
        Console.WriteLine("7) Borrow Item");
        Console.WriteLine("8) Return Item");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }

    static void ListItems()
    {
        if (_service.Items.Count == 0) { Console.WriteLine("No items."); return; }
        Console.WriteLine("Items:");
        foreach (var item in _service.Items)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    static void SearchItems()
    {
        var term = InputHelper.ReadText("Enter search term (empty for all)", allowEmpty: true);
        var results = _service.FindItems(term);
        if (!results.Any()) { Console.WriteLine("No results found."); return; }

        Console.WriteLine("Search results:");
        foreach (var item in results)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");

        var book = _service.AddBook(title, author, pages);
        Console.WriteLine($"Added: {book.GetInfo()}");
    }

    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");

        var mag = _service.AddMagazine(title, issue, publisher);
        Console.WriteLine($"Added: {mag.GetInfo()}");
    }

    static void RegisterMember()
    {
        var name = InputHelper.ReadText("Member name");
        var member = _service.RegisterMember(name);
        Console.WriteLine($"✅ Registered member: {member}");
    }

    static void ListMembers()
    {
        if (_service.Members.Count == 0) { Console.WriteLine("No members registered."); return; }
        Console.WriteLine("Members:");
        foreach (var member in _service.Members)
        {
            Console.WriteLine(member.ToString());
        }
    }

    static void BorrowItem()
    {
        var memberId = InputHelper.ReadInt("Enter member Id");
        var itemId = InputHelper.ReadInt("Enter item Id");

        if (_service.BorrowItem(memberId, itemId, out var message))
            Console.WriteLine($"✅ {message}");
        else
            Console.WriteLine($"❌ {message}");
    }

    static void ReturnItem()
    {
        var memberId = InputHelper.ReadInt("Enter member Id");
        var itemId = InputHelper.ReadInt("Enter item Id");

        if (_service.ReturnItem(memberId, itemId, out var message))
            Console.WriteLine($"✅ {message}");
        else
            Console.WriteLine($"❌ {message}");
    }
}