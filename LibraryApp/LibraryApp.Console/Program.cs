using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Library App!");

bool exit = false;

while (!exit)
{
    ShowMenu();
    var input = Console.ReadLine();
    int choice;
    if (!int.TryParse(input, out choice))
    {
        Console.WriteLine("invalid input. please enter a number.");
        continue;
    }
    Console.WriteLine();

    switch (choice)
    {
        case 1: ListItems(); break;
        default: Console.WriteLine("Unknown option."); break;
    }
 
}

Console.WriteLine("Goodbye!");

static void ShowMenu()
{
    Console.WriteLine("=== Library Management System===");
    Console.WriteLine("1) List all items");

}

static void ListItems()
{
    Console.WriteLine("=== NO items ===");
}