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
            dbContext.Database.EnsureCreated();
            if (dbContext.Products.Count() == 0)
            {
                dbContext.Products.AddRange(
                    new Product { Name = "Product 1", Price = 10.99m, Description = "Description for Product 1" },
                    new Product { Name = "Product 2", Price = 20.99m, Description = "Description for Product 2" },
                    new Product { Name = "Product 3", Price = 30.99m, Description = "Description for Product 3" }
                );
                dbContext.SaveChanges();
            }
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
