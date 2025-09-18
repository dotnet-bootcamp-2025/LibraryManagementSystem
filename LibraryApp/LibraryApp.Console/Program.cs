using LibraryApp.Console.Domain;
using LibraryApp.Console.Services;
using LibraryApp.Console.Utils;

public class Program
{
    private static readonly LibraryService service = new();

    public static void Main()
    {
        Console.WriteLine("Library App!");
        service.Seed();

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


    #region HELPERS

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

    static void PrintItems(IEnumerable<LibraryItem> items)
    {
        if (!items.Any()) { Console.WriteLine("No items."); return; }

        Console.WriteLine("Items:");
        foreach (var item in items)
        {
            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        }
    }

    #endregion HELPERS

    #region ACTIONS

    static void ListItems()
    {
        PrintItems(service.Items);
    }

    static void AddBook()
    {
        var title = InputHelper.ReadText("Title");
        var author = InputHelper.ReadText("Author");
        var pages = InputHelper.ReadInt("Pages (0 if unknown)");

        var book = service.AddBook(title, author, pages);
        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }


    static void SearchItems()
    {
        var term = InputHelper.ReadText(">Search term:", true);

        PrintItems(service.FindItems(term));
    }

    static void AddMagazine()
    {
        var title = InputHelper.ReadText("Title");
        var issue = InputHelper.ReadInt("Issue number");
        var publisher = InputHelper.ReadText("Publisher");

        var mag = service.AddMagazine(title, issue, publisher);
        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }

    static void ListMembers() 
    {

    }
    
    static void RegisterMember() 
    {

    }

    static void BorrowItem() 
    {
        
    }

    static void ReturnItem() 
    {
        
    }

    #endregion ACTIONS
}