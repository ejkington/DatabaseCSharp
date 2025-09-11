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
            string connectionString = "Server=localhost;Database=DatabaseCsharp;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id FROM Orders WHERE SentToOT = 0";
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int registerAddress = 0;
                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(0);
                        modbusClient.WriteSingleRegister(registerAddress, orderId);

                        Console.WriteLine($"Order med Id={orderId} regristrerad till {registerAddress}");
                        registerAddress++;
                    }
                }
            }
            modbusClient.Disconnect();
            Console.WriteLine("Färdigt!");
        }

    }
}
