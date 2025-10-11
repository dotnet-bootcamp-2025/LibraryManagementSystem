using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Infrastructure.Data;
using LibraryApp.Application.Services; // si necesitas registrar impl concreta
using LibraryApp.Console.Utils;        // InputHelper
// ajusta namespaces según tu solución

namespace LibraryApp.ConsoleApp
{
    public class Program
    {
        public static void Main()
        {
            // 1) Configurar DI (ServiceCollection)
            var services = new ServiceCollection();
            

            // Si usas AppDbContext en el servicio, registra DbContext (ajusta connection string)
            var connectionString = "Data Source=libraryapp.db"; // o lee desde config
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            // Registrar repositorios / servicios (las mismas que registras en la API)
            services.AddScoped<ILibraryAppRepository, LibraryAppRepository>();
            services.AddScoped<ILibraryService, LibraryService>();

            var provider = services.BuildServiceProvider();

            // Crear scope y resolver ILibraryService
            using (var scope = provider.CreateScope())
            {
                var svc = scope.ServiceProvider.GetRequiredService<ILibraryService>();

                // Seed demo data usando el servicio resuelto
                SeedDemo(svc);

                System.Console.WriteLine("Library App!");

                bool exit = false;
                while (!exit)
                {
                    ShowMenu();
                    var input = System.Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        System.Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }
                    if (!int.TryParse(input, out int choice))
                    {
                        System.Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue;
                    }
                    System.Console.WriteLine();
                    switch (choice)
                    {
                        case 1: ListItems(svc); break;
                        case 2: SearchItems(svc); break;
                        case 3: AddBook(svc); break;
                        case 4: AddMagazine(svc); break;
                        case 5: ListMembers(svc); break;
                        case 6: RegisterMember(svc); break;
                        case 7: BorrowItem(svc); break;
                        case 8: ReturnItem(svc); break;
                        case 0: exit = true; break;
                        default: System.Console.WriteLine("Unknown option."); break;
                    }
                    if (!exit)
                    {
                        System.Console.WriteLine("\nPress any key to continue...");
                        System.Console.ReadKey();
                        System.Console.Clear();
                    }
                }
                System.Console.WriteLine("Goodbye!");
            }
        }

        static void ShowMenu()
        {
            System.Console.WriteLine("=== Library Management System ===");
            System.Console.WriteLine("1) List all items");
            System.Console.WriteLine("2) Search items by title");
            System.Console.WriteLine("3) Add Book");
            System.Console.WriteLine("4) Add Magazine");
            System.Console.WriteLine("5) List Members");
            System.Console.WriteLine("6) Register Member");
            System.Console.WriteLine("7) Borrow Item");
            System.Console.WriteLine("8) Return Item");
            System.Console.WriteLine("0) Exit");
            System.Console.WriteLine("---------------------------------");
        }

        static void ListItems(ILibraryService service)
        {
            var items = service.GetAllLibraryItems().ToList();
            if (items.Count == 0) { System.Console.WriteLine("No items."); return; }
            System.Console.WriteLine("Items:");
            foreach (var item in items)
            {
                var status = item.IsBorrowed ? "BORROWED" : "AVAILABLE";
                System.Console.WriteLine($"{item.Id}: {item.GetInfo()} [{status}]");
            }
        }

        static void SeedDemo(ILibraryService service)
        {
            // Evitar duplicados si vuelves a seedear
            var existing = service.GetAllLibraryItems();
            if (existing != null && existing.Any()) return;

            service.AddBook("Clean Code", "Robert C. Martin", 464);
            service.AddBook("The Pragmatic Programmer", "Andrew Hunt", 352);
            service.AddMagazine("DotNET Weekly", 120, "DevPub");
            service.AddMagazine("Tech Monthly", 58, "TechPress");

            // // Members
            // service.RegisterMember("Anna", );
            // service.RegisterMember("James");
        }

        static void SearchItems(ILibraryService service)
        {
            var term = InputHelper.ReadText("Search term");
            if (term is null) return;
            var results = service.FindItems(term).ToList();
            System.Console.WriteLine();
            System.Console.WriteLine($"Search results for \"{term}\":");
            foreach (var result in results)
            {
                System.Console.WriteLine(result.GetInfo());
            }
        }

        static void AddBook(ILibraryService service)
        {
            var title = InputHelper.ReadText("Title");
            var author = InputHelper.ReadText("Author");
            var pages = InputHelper.ReadInt("Pages (0 if unknown)");
            var book = service.AddBook(title, author, pages);
            System.Console.WriteLine($"Added: {book.GetInfo()} (Id={book.Id})");
        }

        static void AddMagazine(ILibraryService service)
        {
            var title = InputHelper.ReadText("Title");
            var issue = InputHelper.ReadInt("Issue number");
            var publisher = InputHelper.ReadText("Publisher");
            var mag = service.AddMagazine(title, issue, publisher);
            System.Console.WriteLine($"Added: {mag.GetInfo()} (Id={mag.Id})");
        }

        static void ListMembers(ILibraryService service)
        {
            var members = service.GetAllMembers().ToList();
            if (members.Count == 0) { System.Console.WriteLine("No members."); return; }
            System.Console.WriteLine($"Members: {members.Count}");
            System.Console.WriteLine("ID\tName\tLoans");
            System.Console.WriteLine("-------------------------");

            foreach (var member in members)
            {
                var loans = member.BorrowedItems ?? Enumerable.Empty<object>();
                var status = loans.Any() ? $"LOANS: {loans.Count()}" : "NO BORROWED ITEMS PENDING TO RETURN";
                System.Console.WriteLine($"{member.Id}\t{member.Name}\t{status}");
            }
        }

        static void RegisterMember(ILibraryService service)
        {
            var name = InputHelper.ReadText("Name");
            var member = service.RegisterMember(name, DateTime.Now, DateTime.Now.AddMonths(3));
            System.Console.WriteLine($"Added: {member.Name} (Id={member.Id})");
        }

        static void BorrowItem(ILibraryService service)
        {
            var memberId = InputHelper.ReadInt("Member Id");
            var itemId = InputHelper.ReadInt("Item Id");

            if (!service.BorrowItem(memberId, itemId, out string message))
            {
                System.Console.WriteLine($"Cannot borrow item: {message}");
                return;
            }
            System.Console.WriteLine($"Borrowed: {message}");
        }

        static void ReturnItem(ILibraryService service)
        {
            var memberId = InputHelper.ReadInt("Member Id");
            var itemId = InputHelper.ReadInt("Item Id");

            if (!service.ReturnItem(memberId, itemId, out string message))
            {
                System.Console.WriteLine($"Item to return invalid: {message}");
                return;
            }
            System.Console.WriteLine($"Returned: {message}");
        }
    }
}
