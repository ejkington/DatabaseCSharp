using DatabaseCSharp;

namespace IntegrationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EasyModbus.ModbusClient modbusClient = new EasyModbus.ModbusClient("127.0.0.1", 502);
            modbusClient.Connect();
            //Loopa läsa i databasen - för varje order
            int orderId = 1; //Hämta från databas
            modbusClient.WriteSingleRegister(0, orderId);

            Console.WriteLine("Hello, World!");
        }

    }
}
