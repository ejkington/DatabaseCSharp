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
        public readonly ShopDbContext dbContext;
        
        public ShopApp(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal void Init()
        {
            dbContext.Database.Migrate(); // Skapar/uppdaterar databasen

            var products = new List<Product>
            {
            new Product { Name = "Arctis Nova 1 Wireless for PC - Black", Price = 849, Description = "Trådlöst headset för PC med klar ljudkvalitet." },
            new Product { Name = "Arctis Nova 5P Wired for PlayStation - Red", Price = 1099, Description = "Kabelheadset med balanserat ljud och mikrofon." },
            new Product { Name = "Arctis Nova 3 Wireless for Xbox - Black", Price = 1799, Description = "Bekvämt headset för Xbox med lång batteritid." },
            new Product { Name = "Arctis Nova 7 Wireless for PC & PlayStation - White", Price = 2299, Description = "Premium headset med surroundljud." },
            new Product { Name = "Arctis Nova 9P Wireless for PlayStation - Silver", Price = 2599, Description = "High-end headset med kristallklart ljud." },
            new Product { Name = "Arctis GameBuds™ Pro for PC & PlayStation", Price = 2199, Description = "Små, lätta earbuds med bra ljudisolering." },
            new Product { Name = "Arctis Nova 7+ Wireless for PlayStation - Green", Price = 2099, Description = "Gamingheadset med flexibel mikrofon." },
            new Product { Name = "Arctis 1P Wired for PlayStation - Blue", Price = 699, Description = "Prisvärt headset med bra ljud och enkel anslutning." },
            new Product { Name = "Arctis 3P Wired for PC - Black", Price = 899, Description = "Lätt headset med balanserat ljud för gaming." },
            new Product { Name = "Arctis Nova 9+ Wireless for PC - Gold", Price = 2699, Description = "Exklusivt headset med hög ljudkvalitet och trådlös frihet." }
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
                        ShowAllOrders();
                        break;
                    case "2":
                        AddOrder();
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

        public void ShowAllOrders()
        {
            var orders = dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ToList();

            foreach (var order in orders)
            {
                Console.WriteLine($"\nOrder ID: {order.Id}, Datum: {order.OrderDate}");

                decimal total = 0;
                foreach (var item in order.Items)
                {
                    var lineTotal = item.Quantity * item.Product.Price;
                    total += lineTotal;
                    Console.WriteLine($"  - {item.Product.Name} x{item.Quantity} ({item.Product.Price} kr/st) = {lineTotal} kr");
                }

                Console.WriteLine($"  Totalsumma: {total} kr");
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå...");
            Console.ReadKey();
        }

        public void AddOrder()
        {
            Console.WriteLine("\nTillgängliga produkter:");
            var products = dbContext.Products.ToList();

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Id}. {product.Name} - {product.Price} kr");
            }

            var order = new Order();

            while (true)
            {
                Console.Write("\nAnge produkt-ID (eller lämna tomt för att slutföra): ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;

                if (!int.TryParse(input, out var productId)) continue;
                var product = dbContext.Products.Find(productId);
                if (product == null) continue;

                Console.Write("Ange antal: ");
                if (!int.TryParse(Console.ReadLine(), out var qty)) qty = 1;

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = qty
                });
            }

            if (order.Items.Count > 0)
            {
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();
                Console.WriteLine("\n Ordern lades till!");
            }
            else
            {
                Console.WriteLine("\n Ingen order skapades.");
            }

            Console.WriteLine("Tryck på valfri tangent för att återgå...");
            Console.ReadKey();
        }
    }
}
