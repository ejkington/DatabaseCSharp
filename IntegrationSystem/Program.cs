using DatabaseCSharp;
using EasyModbus;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IntegrationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=DatabaseCsharp;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
            ModbusClient modbusClient = new ModbusClient("127.0.0.1", 1503);

            while (true)
            {
                try
                {
                    // Kontrollera anslutning
                    if (!modbusClient.Connected)
                    {
                        modbusClient.Connect();
                        Console.WriteLine("[Client] Uppkoppling till Modbus lyckad.");
                    }

                    List<int> orderIds = new List<int>();

                    // Hämta alla nya orders
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = "SELECT Id FROM Orders WHERE SentToOT = 0";
                        SqlCommand selectCommand = new SqlCommand(selectQuery, connection);

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderIds.Add(reader.GetInt32(0));
                            }
                        }

                        // Skicka orders till Modbus och markera som skickade
                        foreach (var orderId in orderIds)
                        {
                            // Skickar alltid till samma register (0)
                            modbusClient.WriteSingleRegister(0, orderId);

                            MarkOrderAsSent(orderId, connection);
                            Console.WriteLine($"[Client] Order {orderId} skickad.");
                        }
                    }

                    Thread.Sleep(5000); // Vänta 5 sekunder innan nästa loop
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Client] Error: {ex.Message}");
                    Console.WriteLine("[Client] Försöker igen om 2Sek...");
                    Thread.Sleep(2000);
                }
            }
        }

        private static void MarkOrderAsSent(int orderId, SqlConnection connection)
        {
            string updateQuery = "UPDATE Orders SET SentToOT = 1 WHERE Id = @id";
            using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
            {
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine($"[Client] Packar order {orderId}");
            Thread.Sleep(2000);
            
        }
    }
}
