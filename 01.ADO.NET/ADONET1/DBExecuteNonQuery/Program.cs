using System;
using System.Data.SqlClient;

namespace DBExecuteNonQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=SoftUni; Integrated Security=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var commandQuery = "UPDATE Employees SET Salary = Salary * 1.15";
                var updatedSalary = new SqlCommand(commandQuery, connection);

                int result = (int)updatedSalary.ExecuteNonQuery();

                Console.WriteLine($"Salary updated for{result} employee(s).");
            }
        }
    }
}
