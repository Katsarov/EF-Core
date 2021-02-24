
namespace ADONET
{
    using System;
    using System.Data.SqlClient;

    public class Program
    {
        const string connectionString = "Server=.; Database=MinionsDB; Integrated Security=true";

        public static void Main(string[] args)
        {
            NewMethod();
        }

        private static void NewMethod()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createDatabase = "CREATE DATABASE MinionsDB";
                ExecuteNonQuery(connection, "");


            }
        }

        private static void ExecuteNonQuery(SqlConnection connection, string query)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string[] GetCreateTableStatemements()
        {
            var result = new string[]
            {
                "CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50))",
                "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50),CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",
                "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50),Age INT,TownId INT FOREIGN KEY REFERENCES Towns(Id))",
                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50))",
                "TABLE Villains(Id INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50),EvilnessFactoriD INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",
                "CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))"
            };

            return result;
        }
    }
}
