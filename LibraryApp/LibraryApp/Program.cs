using LibraryApp.Domain;
using LibraryApp.ConsoleApp.Utils;
using LibraryApp.Services;

public class Program
{
    private static readonly List<LibraryItem> _items = new();
    private static readonly LibraryService _service = new();
    public static void Main()
    {
        Console.WriteLine("Library App!");
        _service.Seed();
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
    static void ShowMenu()
    {
        Console.WriteLine("=== Library Management System ===");
        Console.WriteLine("1) List all items");
        Console.WriteLine("2) Search items by title (TBD)");
        Console.WriteLine("3) Add Book");
        Console.WriteLine("4) Add Magazine");
        Console.WriteLine("5) List Members");
        Console.WriteLine("6) Add Member");
        Console.WriteLine("7) Borrow item");
        Console.WriteLine("8) Return item");
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
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }
    static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");

        var book = _service.AddBook(title, author, pages);
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }
    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");

        //var _nextItemId = _items.Count > 0 ? _items.Max(i => i.Id) : 0;
        var mag = _service.AddMagazine(title, issue, publisher);
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }

    private static void RegisterMember()
    {
        var name = InputHelper.ReadText("Member name");
        var member = _service.RegisterMember(name);
        Console.WriteLine($"Registered: {member}");
    }

    private static void ListMembers()
    {
        if (_service.Members.Count == 0) { Console.WriteLine("No members."); return; }
        Console.WriteLine("Members: ");
        foreach (var member in _service.Members)
        {
            Console.WriteLine($"{member.Name} (Borrowed: {member.BorrowedItems.Count})");
        }
    }

    private static void SearchItems()
    {
        var term = InputHelper.ReadText("Search term");
        Console.WriteLine($"Search results for \"{term}\":");
        InputHelper.PrintList(_service.FindItems(term));
    }

    private static void BorrowItem()
    {
        ListMembers();
        var idMember = InputHelper.ReadInt("Member id");
        ListItems();
        var idItem = InputHelper.ReadInt("Item id");
        string resultMessage;

        _service.BorrowItem(idMember, idItem, out resultMessage);

        Console.WriteLine($"{resultMessage}");
    }

    private static void ReturnItem()
    {
        ListMembers();
        var idMember = InputHelper.ReadInt("Member id");
        ListItems();
        var idItem = InputHelper.ReadInt("Item id");
        string resultMessage;

        _service.ReturnItem(idMember, idItem, out resultMessage);

        Console.WriteLine($"{resultMessage}");
    }
}