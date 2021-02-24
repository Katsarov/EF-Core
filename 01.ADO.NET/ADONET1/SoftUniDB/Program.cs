using System;
using System.Data.SqlClient;

namespace DBExecuteScalar
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=SoftUni;Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandQuery = "SELECT COUNT(*) FROM Employees";

                var command = new SqlCommand(commandQuery, connection);

                int result = (int)command.ExecuteScalar();

                Console.WriteLine(result);
            }
        }
    }
}
