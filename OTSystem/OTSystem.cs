using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystemOT
{
    internal class OTSystem
    {
        private List<OrderDTO> pendingOrders = new List<OrderDTO>();

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
                pendingOrders.Clear();
                Thread.Sleep(10000); // Vänta 10 sekunder innan nästa kontroll
            }
        }


    }
}
