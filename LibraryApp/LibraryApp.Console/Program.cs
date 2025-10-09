//using LibraryApp.Console.Domain;
//using LibraryApp.Console.Services;
using LibraryApp.Application.Services;
using LibraryApp.Console.Utils;
using LibraryApp.Domain;
public class Program
{
    private static readonly List<LibraryItem> _items = new();
   // private static readonly LibraryService _service = new();
    public static void Main()
    {
        Console.WriteLine("Library App!");
        //_service.Seed();
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
                case 5: MemberList(); break;
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
        Console.WriteLine("5) List Member");
        Console.WriteLine("6) Register Member");
        Console.WriteLine("7) Borrow Item");
        Console.WriteLine("8) Return Item");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }
    static void ListItems()
    {
        //if (_service.Items.Count == 0) { Console.WriteLine("No items."); return; }
        //Console.WriteLine("Items:");
        //foreach (var item in _service.Items)
        //{
        //    var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
        //    // Polymorphism: each derived class presents info differently
        //    Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        //}
    }

    static void SearchItems()
    {
        //var term = InputHelper.ReadText("Search term (part of title)");
        //var results = _service.FindItems(term); // from LibraryService
        //if (!results.Any()) { Console.WriteLine("No items found."); return; }
        //Console.WriteLine("Search Results:");
        //foreach (var item in results)
        //{
        //    var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
        //    Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        //}
    }
    static void AddBook()
    {
        //var title = InputHelper.ReadText("Title");
        //var author = InputHelper.ReadText("Author");
        //var pages = InputHelper.ReadInt("Pages (0 if unknown)");
        //var book = _service.AddBook(title, author, pages); // from LibraryService
        //Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }
    static void AddMagazine()
    {
        //var title = InputHelper.ReadText("Title");
        //var issue = InputHelper.ReadInt("Issue number");
        //var publisher = InputHelper.ReadText("Publisher");
        //var mag = _service.AddMagazine(title, issue, publisher); // from LibraryService
        //Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }
    static void MemberList()
    {
        //var members = _service.Members; // from LibraryService
        //if (members.Count == 0) { Console.WriteLine("No members."); return; }
        //Console.WriteLine("Members:");
        //foreach (var member in members)
        //{
        //    Console.WriteLine($"{member.Id}: {member.Name}");
        //}
    }

    static void RegisterMember()
    {
        //var name = InputHelper.ReadText("Member Name");
        //var member = _service.RegisterMember(name); // from LibraryService
        //Console.WriteLine($"Registered Member: {member.Name} (Id={member.Id})");
    }

    static void BorrowItem()
    {
        //var memberId = InputHelper.ReadInt("Member Id");
        //var itemId = InputHelper.ReadInt("Item Id");
        //if (_service.BorrowItem(memberId, itemId, out var message)) // from LibraryService
        //{
        //    Console.WriteLine("Success: " + message);
        //}
        //else
        //{
        //    Console.WriteLine("Error: " + message);
        //}
    }

    static void ReturnItem()
    {
        //var memberId = InputHelper.ReadInt("Member Id");
        //var itemId = InputHelper.ReadInt("Item Id");
        //if (_service.ReturnItem(memberId, itemId, out var message)) // from LibraryService
        //{
        //    Console.WriteLine("Success: " + message);
        //}
        //else
        //{
        //    Console.WriteLine("Error: " + message);
        //}
    }

}