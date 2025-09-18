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
        default: Console.WriteLine("Unknown option."); break;
    }
}
Console.WriteLine("Goodbye!");
static void ShowMenu()
{
    Console.WriteLine("=== Library Management System ===");
    Console.WriteLine("1) List all items");
    Console.WriteLine("---------------------------------");
}
static void ListItems()
{
    Console.WriteLine("=== NO items ===");
}












