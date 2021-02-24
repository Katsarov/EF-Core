using System;
using System.Data.SqlClient;

namespace AdoNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.; Database=SoftUni; Integrated Security=true";

            //  ExecuteScalar

            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    string command = "SELECT COUNT(*) FROM Employees";
            //    var sqlCommand = new SqlCommand(command, sqlConnection);
            //    //object result = sqlCommand.ExecuteScalar();
            //    int result = (int)sqlCommand.ExecuteScalar();
            //    Console.WriteLine(result);
            //}


            //  ExecuteReader

            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    string command = "SELECT FirstName, LastName  FROM Employees WHERE FirstName LIKE 'N%'";
            //    var sqlCommand = new SqlCommand(command, sqlConnection);
            //    SqlDataReader reader = sqlCommand.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        string firstName = (string)reader["FirstName"];
            //        string lastName = (string)reader["LastName"];
            //        Console.WriteLine(firstName + " " + lastName);
            //    } 
            //}


            //  ExecuteNonQuery

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string command = "SELECT FirstName, LastName, Salary  FROM Employees WHERE FirstName LIKE 'N%'";
                var sqlCommand = new SqlCommand(command, sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal salary = (decimal)reader["Salary"];
                        Console.WriteLine(firstName + " " + lastName + " => " + salary);
                    }
                }

                SqlCommand updateSalaryCommand = new SqlCommand(
                    "UPDATE Employees SET Salary = Salary * 1.1", sqlConnection);
                int updatedRows = updateSalaryCommand.ExecuteNonQuery();
                Console.WriteLine($"Salary updated for {updatedRows} employee(s).");

                var reader2 = sqlCommand.ExecuteReader();
                using (reader2)
                {
                    while (reader2.Read())
                    {
                        string firstName = (string)reader2["FirstName"];
                        string lastName = (string)reader2["LastName"];
                        decimal salary = (decimal)reader2["Salary"];
                        Console.WriteLine(firstName + " " + lastName + " => " + salary);
                    }
                }
            }


        }
    }
}
