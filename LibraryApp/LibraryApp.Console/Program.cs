using LibraryApp.Console.Domain;
using LibraryApp.Console.Services;
using LibraryApp.Console.Utils;
public class Program
{
    // private static readonly List<LibraryItem> _items = new();
    private static readonly LibraryService _service = new();
    public static void Main()
    {
        SeedDemo();
        
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
                case 5: RegisterMember(); break;
                case 6: BorrowItem(); break;
                case 7: ReturnItem(); break;
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
        Console.WriteLine("6) Borrow Item");
        Console.WriteLine("7) Return Item");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }
    static void ListItems()
    {//var items instead of _items 
        var items = _service.Items;
        if (items.Count == 0) { Console.WriteLine("No items."); return; }
        Console.WriteLine("Items:");
        foreach (var item in items)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }
    // Seed fake data for the demo
    static void SeedDemo()
    { //se eliminan numeros id al marcar error
        _service.AddBook("Clean Code", "Robert C. Martin", 464);
        _service.AddBook("The Pragmatic Programmer", "Andrew Hunt", 352);
        _service.AddMagazine("DotNET Weekly", 120, "DevPub");
        _service.AddMagazine("Tech Monthly", 58, "TechPress");
        _service.RegisterMember("Bob");
    }
    
    static void SearchItems()
    { 
        var term = InputHelper.ReadText("Search term");
        if (term is null) return;
        var results = _service.FindItems(term).ToList();
        Console.WriteLine();
        Console.WriteLine($"Search results for \"{term}\":");
        foreach (var result in results)
        {
            Console.WriteLine(result.GetInfo());
        }
    }
    
    static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");
        // var _nextItemId = _items.Count > 0 ? _items.Max(i => i.Id): 0;
        var book = _service.AddBook( title, author, pages);
        // _items.Add(book);
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }
    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");
        // var _nextItemId = _items.Count > 0 ? _items.Max(i => i.Id) : 0;
        var mag = _service.AddMagazine( title, issue, publisher); //using _service
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }
    
    static void RegisterMember()
    {
        var name = InputHelper.ReadText("Name");
        //var _nextMemberId = _service.Members.Count > 0 ? _service.Members.Max(m => m.Id) : 0;
        var member = _service.RegisterMember( name);
       //Name ! GetInfo
        Console.WriteLine($"Added: {member.Name} (Id={member.Id})");
    }
    
    static void BorrowItem()
    {
        var memberId = InputHelper.ReadInt("Member Id");
        var itemId = InputHelper.ReadInt("Item Id");
        
        if(!_service.BorrowItem(memberId, itemId, out string message))
        {
            Console.WriteLine($"Cannot borrow item: {message}");
            return;
        }
        Console.WriteLine($"Borrowed: {message}");
    }
    
    static void ReturnItem()
    {
        var memberId = InputHelper.ReadInt("Member Id");
        var itemId = InputHelper.ReadInt("Item Id");

        if (!_service.ReturnItem(memberId,itemId, out string message))
        {
            Console.WriteLine($"Item to return invalid: {message}");
            return;
        }
        Console.WriteLine($"Returned: {message}");
    }
}










