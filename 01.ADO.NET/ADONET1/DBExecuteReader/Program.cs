using System;
using System.Data.SqlClient;

namespace DBExecuteReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=SoftUni; Integrated Security=true";

            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var commandQuery = "SELECT FirstName, LastName FROM Employees WHERE FirstName LIKE 'N%'";
                var command = new SqlCommand(commandQuery, connection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string firstName = (string)reader["FirstName"];
                    string lastName = (string)reader["LastName"];

                    Console.WriteLine(firstName + " " + lastName);
                }
            }
        }
    }
}
