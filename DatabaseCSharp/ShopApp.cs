using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCSharp
{
    internal class ShopApp
    {
        private readonly ShopDbContext dbContext;
        
        public ShopApp(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal void Init()
        {
            dbContext.Database.Migrate(); // Skapar/uppdaterar databasen

            var products = new List<Product>
            {
            new Product { Name = "Arctis Nova Pro Wireless for PC & PlayStation", Price = 3199, Description = "..." },
            new Product { Name = "Arctis Nova 3P Wireless for PlayStation - Black", Price = 1299, Description = "..." },
            new Product { Name = "Arctis GameBuds™ for PlayStation", Price = 1999, Description = "..." },
            new Product { Name = "Arctis Nova 1 for PlayStation - Black", Price = 799, Description = "..." }
            };

            // Lägg till eller uppdatera seed-produkter
            foreach (var product in products)
            {
                var existing = dbContext.Products.FirstOrDefault(p => p.Name == product.Name);
                if (existing == null)
                {
                    dbContext.Products.Add(product);
                }
                else
                {
                    existing.Price = product.Price;
                    existing.Description = product.Description;
                }
            }

            dbContext.SaveChanges();
        }

        internal void RunMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Meny ---");
                Console.WriteLine("1. Visa alla ordrar");
                Console.WriteLine("2. Lägg till order");
                Console.WriteLine("3. Skapa Ticket i JIRA");
                Console.WriteLine("4. Avsluta");
                Console.Write("Val: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllProducts();
                        break;
                    case "2":
                        AddProduct();
                        return;
                    case "3":
                        CreateJiraTicket();
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
            }
        }

        // Funktion för att skapa JIRA-ticket automatiskt via API.
        private void CreateJiraTicket()
        {
            Console.WriteLine("\nFunktion för att skapa JIRA-ticket är inte implementerad ännu.");
            Console.WriteLine("Tryck på valfri tangent för att återgå till menyn...");
            Console.ReadLine();
            RunMenu();
        }

        private void ShowAllProducts()
        {
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"Order ID: {product.Id}, Namn: {product.Name}, Pris: {product.Price}KR, Beskrivning: {product.Description}");
            }
        }

        private void AddProduct()
        {
            Console.Write("\nAnge namn på produkten: ");
            var name = Console.ReadLine();

            Console.Write("Ange pris: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price))
            {
                Console.WriteLine("Ogiltigt pris.");
                return; // går direkt tillbaka till menyn
            }

            Console.Write("Ange beskrivning: ");
            var description = Console.ReadLine();

            var newProduct = new Product
            {
                Name = name,
                Price = price,
                Description = description
            };

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();

            Console.WriteLine($"\n✅ Produkten \"{name}\" lades till!");
            Console.WriteLine("Tryck på valfri tangent för att återgå till menyn...");
            Console.ReadLine();
            RunMenu();
        }
    }
}
