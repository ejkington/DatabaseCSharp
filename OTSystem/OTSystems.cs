using DatabaseCSharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemOT
{

    // Order DTO Lista
    internal class OTSystems
    {
        private List<OrderDTO> pendingOrders = new List<OrderDTO>();


        // Metdod för att Starta OT-systemet
        public void Start()
        {
            Console.WriteLine("\n=== [OT] System Startat === ");

            while(true)
            {
                Console.WriteLine("\n=== [OT] Väntar på order === ");
                FetchordersFromIT();

                foreach (var order in pendingOrders)
                {
                    ProcessOrder(order);
                }

                Console.WriteLine("=== [OT] Alla väntande ordrar har behandlats === ");
                pendingOrders.Clear(); // Rensa listan efter bearbetning
                Thread.Sleep(10000); // Vänta 10 sekunder innan nästa kontroll
            }
        }

        // Metod för att hämta ordrar från IT-systemet
        private void FetchordersFromIT()
        {
            using var dbContext = new ShopDbContext(GetDbContextOptions());

            var newOrders = dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => !o.SentToOT)
                .ToList();

            foreach (var order in newOrders)
            {
                foreach (var item in order.Items)
                {
                    var newOrderDTO = new OrderDTO
                    {
                        OrderId = order.Id,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                    };

                    pendingOrders.Add(newOrderDTO);
                    
                    Console.WriteLine($"[OT] Ny order mottagen: {newOrderDTO.OrderId} - {newOrderDTO.ProductName} X {newOrderDTO.Quantity}");
                }

                order.SentToOT = true;
            }

            dbContext.SaveChanges();
        }

        // Metod för att processa en order
        private void ProcessOrder(OrderDTO order)
        {
            Console.WriteLine($"[OT] Bearbetar order {order.OrderId}: {order.ProductName} X {order.Quantity}");
            Thread.Sleep(2000);
            Console.WriteLine($"[OT] Order {order.OrderId} klar!");
        }

        private DbContextOptions<ShopDbContext> GetDbContextOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=DatabaseCsharp;Trusted_Connection=True;TrustServerCertificate=True;");
            return optionsBuilder.Options;
        }

        internal class OrderDTO
        {
            public int OrderId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; } //
        }
    }
}
