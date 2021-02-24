using System;
using System.Data.SqlClient;
using System.Text;

namespace P03.MinionNames
{
    public class StartUp
    {
        static void Main(string[] args)
        {

            string connectionString = @"Server=.;Database=MinionsDB; Integrated Security=true";

            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                //read input
                int villainId = int.Parse(Console.ReadLine());

                string result = GetMinionsInfoAboutVillain(sqlConnection, villainId);

                Console.WriteLine(result);
            }
        }

        private static string GetMinionsInfoAboutVillain(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder sb = new StringBuilder();

            string villainName = GetVillainName(sqlConnection, villainId);

            if(villainName == null)
            {
                sb.AppendLine($"No villain with ID {villainId} exists in the database.");
            }
            else
            {
                sb.AppendLine($"Villain: {villainName}");

                string getMinionsInfoQuery = @"SELECT m.Name, m.Age
	                                                FROM Villains v
	                                                LEFT JOIN MinionsVillains mv ON v.Id = mv.VillainId
	                                                LEFT JOIN Minions m ON mv.MinionId = m.Id
	                                              WHERE v.Name = @villainName
	                                              ORDER BY m.Name";

                SqlCommand commandMinionsInfo = new SqlCommand(getMinionsInfoQuery, sqlConnection);
                commandMinionsInfo.Parameters.AddWithValue("@villainName", villainName);

                using SqlDataReader reader = commandMinionsInfo.ExecuteReader();


                if (reader.HasRows)
                {
                    int rowNum = 1;
                    while (reader.Read())
                    {
                        string minionName = reader["Name"]?.ToString();
                        string minionAge = reader["Age"]?.ToString();

                        sb
                            .AppendLine($"{rowNum}. {minionName} {minionAge}");

                        rowNum++;
                    }
                }
                else
                {
                    sb
                        .AppendLine("(no minions)");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static string GetVillainName(SqlConnection connectionSql, int villainId) 
        {
            string getVillainNameQuery = @"SELECT Name FROM Villains
                                                WHERE Id = @villainId";

            using SqlCommand commandVillainName = new SqlCommand(getVillainNameQuery, connectionSql);
            commandVillainName.Parameters.AddWithValue("@villainId", villainId);

            string villainName = commandVillainName.ExecuteScalar()?.ToString();

            return villainName;
        }
    }
}
