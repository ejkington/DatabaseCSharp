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
            dbContext.Database.Migrate(); // Kör migrations, skapar/updaterar schema

            var products = new List<Product>
    {
        new Product { Name = "Arctis Nova Pro Wireless for PC & PlayStation", Price = 3199, Description = "..." },
        new Product { Name = "Arctis Nova 3P Wireless for PlayStation - Black", Price = 1299, Description = "..." },
        new Product { Name = "Arctis GameBuds™ for PlayStation", Price = 1999, Description = "..." },
        new Product { Name = "Arctis Nova 1 for PlayStation - Black", Price = 799, Description = "..." }
    };

            // Hämta alla produkter i databasen
            var existingProducts = dbContext.Products.ToList();

            // Lägg till eller uppdatera
            foreach (var product in products)
            {
                var existing = existingProducts.FirstOrDefault(p => p.Name == product.Name);
                if (existing == null)
                {
                    // Ny produkt → lägg till
                    dbContext.Products.Add(product);
                }
                else
                {
                    // Uppdatera om något ändrats
                    existing.Price = product.Price;
                    existing.Description = product.Description;
                }
            }

            // Ta bort produkter som inte finns i koden längre
            foreach (var dbProduct in existingProducts)
            {
                if (!products.Any(p => p.Name == dbProduct.Name))
                {
                    dbContext.Products.Remove(dbProduct);
                }
            }

            dbContext.SaveChanges();
        }

        internal void RunMenu()
        {
            Console.WriteLine("Välkommen!");
            foreach (var product in dbContext.Products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Description: {product.Description}");
            }
        }
    }
}
