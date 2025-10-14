public class Program
{
    //private static readonly ILibraryService libraryService = new LibraryService();
    public static void Main()
    {
        Console.WriteLine("Library App!");

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
        Console.WriteLine("2) Search items by title (TBD)");
        Console.WriteLine("3) Add Book");
        Console.WriteLine("4) Add Magazine");
        Console.WriteLine("5) ListMembers");
        Console.WriteLine("6) RegisterMember");
        Console.WriteLine("7) Borrow an Item");
        Console.WriteLine("8) Return an Item");
        Console.WriteLine("0) Exit");
        Console.WriteLine("---------------------------------");
    }

    private static void ListItems()
    {
        //var _items = libraryService.Items;
        //if (_items.Count == 0) { Console.WriteLine("No items."); return; }
        //Console.WriteLine("Items:");
        //foreach (var item in _items)
        //{
        //    var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
        //    // Polymorphism: each derived class presents info differently
        //    Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
        //}
    }

    private static void AddBook()
    {
        //var title = InputHelper.ReadText("Title");
        //var author = InputHelper.ReadText("Author");
        //var pages = InputHelper.ReadInt("Pages (0 if unknown)");
        //var book = libraryService.AddBook(title, author, pages);

        //Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
    }

    private static void AddMagazine()
    {
        //var title = InputHelper.ReadText("Title");
        //var issue = InputHelper.ReadInt("Issue number");
        //var publisher = InputHelper.ReadText("Publisher");
        //var mag = libraryService.AddMagazine(title, issue, publisher);

        //Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
    }

    private static void BorrowItem()
    {
        //var itemId = InputHelper.ReadInt("Enter the ID of the item to borrow");
        //var memberId = InputHelper.ReadInt("Enter the ID of the member borrowing the item");

        //try
        //{
        //    libraryService.BorrowItem(memberId, itemId);
        //    Console.WriteLine($"\nSuccess: Item {itemId} borrowed by member {memberId}.");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"\nError: {ex.Message}");
        //}
    }

    private static void ReturnItem()
    {
        //var itemId = InputHelper.ReadInt("Enter the ID of the item to return");
        //var member = libraryService.Members.FirstOrDefault(m => m.BorrowedItems.Any(i => i.Id == itemId));

        //if (member is null)
        //{
        //    Console.WriteLine("\nError: Could not find a member who has borrowed this item.");
        //    return;
        //}

        //try
        //{
        //    libraryService.ReturnItem(member.Id, itemId);
        //    Console.WriteLine($"\nSuccess: Item {itemId} returned by member {member.Name}.");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"\nError: {ex.Message}");
        //}
    }

    private static void ListMembers()
    {
        //Console.WriteLine("--- Member List ---");
        //var members = libraryService.Members;
        //if (!members.Any())
        //{
        //    Console.WriteLine("No members registered.");
        //    return;
        //}
        //foreach (var member in members)
        //{
        //    Console.WriteLine($"ID: {member.Id}, Name: {member.Name}, Borrowed Items: {member.BorrowedItems.Count}");
        //}
    }

    private static void RegisterMember()
    {
        //Console.WriteLine("--- Register New Member ---");
        //var name = InputHelper.ReadText("Enter member's name");
        //var member = libraryService.RegisterMember(name);
        //Console.WriteLine($"\nRegistered new member: {member.Name} (ID: {member.Id})");
    }
}