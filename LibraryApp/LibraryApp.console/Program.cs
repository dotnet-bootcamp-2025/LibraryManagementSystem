//using LibraryApp.console.Domain;
//using LibraryApp.console.Services;
using LibraryApp.Domain;
using LibraryApp.Services;

public class Program
{
    private static readonly List<LibraryItem> _items = new();
    private static readonly List<Member> _members = new();
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
                case 6: RegiterMember(); break;
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
        Console.WriteLine("6) Register Member");
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
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    static void SearchItems()
    {
        var query = InputHelper.ReadText("Enter title to search");
        var results = _service.Items.Where(i => i.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No items found.");
            return;
        }
        Console.WriteLine($"Found {results.Count} item(s):");
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
       
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }
    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");

        var mag = _service.AddMagazine(title, issue, publisher);
        
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }

    private static void RegiterMember()
    {
        var name = InputHelper.ReadText("Member Name");
        var member = _service.RegisterMember(name);
        Console.WriteLine($"Registered: {member}");
    }

    static void ListMembers()
    {
        if(_service.Members.Count == 0) { Console.WriteLine("No members."); return; }
        Console.WriteLine("Members:");
        foreach (var member in _service.Members)
        {
            var status = member.BorrowedItems.Count > 0 ? "HAS BORROWED ITEMS" : "NO BORROWED ITEMS";
            // Polymorphism: each derived class presents info differently
            Console.WriteLine($"{member.Id}: {member.Name} [{status}]");
        }
    }

    static void FindItems()
    {
        var term = InputHelper.ReadText("Search term (leave empty to list all)");
        var results = string.IsNullOrWhiteSpace(term) ? _items : _items.Where(i => i.Title.Contains(term, StringComparison.OrdinalIgnoreCase));
        if (!results.Any())
        {
            Console.WriteLine("No items found.");
            return;
        }
        Console.WriteLine($"Found {results.Count()} item(s):");
        foreach (var item in results)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

   
    static void BorrowItem()
    {
        // 1. Pedir los datos al usuario.
        Console.WriteLine("=== Borrow Item ===");
        Console.Write("Enter Member ID: ");
        var memberIdString = Console.ReadLine();
        Console.Write("Enter Item ID: ");
        var itemIdString = Console.ReadLine();

        // 2. Validar y convertir la entrada.
        if (!int.TryParse(memberIdString, out var memberId))
        {
            Console.WriteLine("Invalid Member ID. Please enter a valid number.");
            return;
        }

        if (!int.TryParse(itemIdString, out var itemId))
        {
            Console.WriteLine("Invalid Item ID. Please enter a valid number.");
            return;
        }

        // 3. Llamar al servicio.
        string message;
        var success = _service.BorrowItem(memberId, itemId, out message);

        // 4. Mostrar el resultado al usuario.
        if (success)
        {
            Console.WriteLine($"Success: {message}");
        }
        else
        {
            Console.WriteLine($"Error: {message}");
        }
    }

    static void ReturnItem()
    {
        // 1. Get input from the user.
        Console.WriteLine("=== Return Item ===");
        Console.Write("Enter Member ID: ");
        var memberIdString = Console.ReadLine();
        Console.Write("Enter Item ID: ");
        var itemIdString = Console.ReadLine();

        // 2. Validate and convert the input.
        if (!int.TryParse(memberIdString, out var memberId))
        {
            Console.WriteLine("Invalid Member ID. Please enter a valid number.");
            return;
        }

        if (!int.TryParse(itemIdString, out var itemId))
        {
            Console.WriteLine("Invalid Item ID. Please enter a valid number.");
            return;
        }

        // 3. Call the service to perform the return operation.
        string message;
        var success = _service.ReturnItem(memberId, itemId, out message);

        // 4. Print the result to the console.
        if (success)
        {
            Console.WriteLine($"Success: {message}");
        }
        else
        {
            Console.WriteLine($"Error: {message}");
        }
    }
}