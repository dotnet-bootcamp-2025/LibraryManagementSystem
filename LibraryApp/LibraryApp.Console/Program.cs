//using LibraryApp.Domain;
//using LibraryApp.Console.Utils;
//using LibraryApp.Application.Services;

///
//COMMENTED ALL THIS CODE TO AVOID GETTING ERRORS WHILE BUILDING, LIBRARY SERVICE AND DB IMPLEMENTATION HAVE BEEN SUCCESSFULLY IMPLEMENTED ON SWAGGER UI.
///

public class Program
{
    //private static readonly LibraryService service = new();

    public static void Main()
    {
        //Console.WriteLine("Library App!");
        //service.Seed();

        //bool exit = false;
        //while (!exit)
        //{
        //    ShowMenu();
        //    var input = Console.ReadLine();

        //    if (string.IsNullOrWhiteSpace(input))
        //    {
        //        Console.WriteLine("Invalid input. Please enter a number.");
        //        continue;
        //    }

        //    int choice;
        //    if (!int.TryParse(input, out choice))
        //    {
        //        Console.WriteLine("Invalid input. Please enter a valid number.");
        //        continue;
        //    }

        //    Console.WriteLine();
        //    switch (choice)
        //    {
        //        case 1: ListItems(); break;
        //        case 2: SearchItems(); break;
        //        case 3: AddBook(); break;
        //        case 4: AddMagazine(); break;
        //        case 5: ListMembers(); break;
        //        case 6: RegisterMember(); break;
        //        case 7: BorrowItem(); break;
        //        case 8: ReturnItem(); break;
        //        case 0: exit = true; break;
        //        default: Console.WriteLine("Unknown option."); break;
        //    }

        //    if (!exit)
        //    {
        //        Console.WriteLine("\nPress any key to continue...");
        //        Console.ReadKey();
        //        Console.Clear();
        //    }
        //}

        //Console.WriteLine("Goodbye!");

    }
}


//    #region HELPERS

//    static void ShowMenu()
//    {
//        Console.WriteLine("=== Library Management System ===");
//        Console.WriteLine("1) List all items");
//        Console.WriteLine("2) Search items by title (TBD)");
//        Console.WriteLine("3) Add Book");
//        Console.WriteLine("4) Add Magazine");
//        Console.WriteLine("5) List Members");
//        Console.WriteLine("6) Register Member");
//        Console.WriteLine("7) Borrow Item");
//        Console.WriteLine("8) Return Item");
//        Console.WriteLine("0) Exit");
//        Console.WriteLine("---------------------------------");
//    }

//    static void PrintItems(IEnumerable<LibraryItem> items)
//    {
//        if (!items.Any()) { Console.WriteLine("No items."); return; }

//        Console.WriteLine("Items:");
//        foreach (var item in items)
//        {
//            var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
//            Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
//        }
//    }

//    static void PrintMembers(IEnumerable<Member> members)
//    {
//        if (!members.Any()) { Console.WriteLine("No members."); return; }

//        Console.WriteLine("Members:");
//        foreach (var member in members)
//        {
//            Console.WriteLine($"{member.Id}: {member.Name}");
//        }
//    }

//    #endregion HELPERS

//    #region ACTIONS

//    static void ListItems()
//    {
//        PrintItems(service.Items);
//    }

//    static void AddBook()
//    {
//        var title = InputHelper.ReadText("Title");
//        var author = InputHelper.ReadText("Author");
//        var pages = InputHelper.ReadInt("Pages (0 if unknown)");

//        var book = service.AddBook(title, author, pages);
//        Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
//    }

//    static void SearchItems()
//    {
//        var term = InputHelper.ReadText("Search term", true);

//        PrintItems(service.FindItems(term));
//    }

//    static void AddMagazine()
//    {
//        var title = InputHelper.ReadText("Title");
//        var issue = InputHelper.ReadInt("Issue number");
//        var publisher = InputHelper.ReadText("Publisher");

//        var mag = service.AddMagazine(title, issue, publisher);
//        Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
//    }

//    static void ListMembers() 
//    {
//        PrintMembers(service.Members);
//    }

//    static void RegisterMember() 
//    {
//        var name = InputHelper.ReadText("New member name");

//        var member = service.RegisterMember(name);
//        Console.WriteLine($"Registered: {member.Name} (Id={member.Id})");
//    }

//    static void BorrowItem() 
//    {
//        var memberId = InputHelper.ReadInt("Member Id");
//        var itemId = InputHelper.ReadInt("Item Id");
//        var message = string.Empty;

//        var result = service.BorrowItem(memberId, itemId, out message);

//        if (!result) Console.WriteLine("Sorry, an error has occurred:");

//        Console.WriteLine(message);
//    }

//    static void ReturnItem() 
//    {
//        var memberId = InputHelper.ReadInt("Member Id");
//        var itemId = InputHelper.ReadInt("Item Id");
//        var message = string.Empty;

//        var result = service.ReturnItem(memberId, itemId, out message);

//        if (!result) Console.WriteLine("Sorry, an error has occurred:");

//        Console.WriteLine(message);
//    }

//    #endregion ACTIONS
//}