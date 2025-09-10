using DatabaseCSharp;
using Microsoft.Data.SqlClient;

namespace IntegrationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EasyModbus.ModbusClient modbusClient = new EasyModbus.ModbusClient("127.0.0.1", 502);
            modbusClient.Connect();

            // Koppla upp till databasen
            string connectionString = "Server=localhost;Database=OrdersDB;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Orders WHERE SentToOT = 0";
                SqlCommand command = new SqlCommand(query, connection);
            }
        }

    }
}
