using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace P04.AddMinion
{
    public class StartUp
    {
        private const string connectionString = @"Server=.;Database=MinionsDB; Integrated Security=true";

        static void Main(string[] args)
        {
            using var sqlConnection = new SqlConnection(connectionString);

            sqlConnection.Open();

            //Прочитаме и си сплитваме шходните данни
            string[] minionInput = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            string[] minionsInfo = minionInput[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();

            string[] villainInput = Console.ReadLine().Split(": ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            string[] villainInfo = villainInput[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();


            //Правим си метод, който ни връща резултат
            string result = AddMinionToDatabase(sqlConnection, minionsInfo, villainInfo);

            Console.WriteLine(result);
        }

        private static string AddMinionToDatabase(SqlConnection sqlConnection, string[] minionsInfo, string[] villainInfo)
        {
            StringBuilder output = new StringBuilder();

            string minionName = minionsInfo[0];
            string minionAge = minionsInfo[1];
            string minionTown = minionsInfo[2];
            //string minionCountryCode = minionsInfo[3];

            string villainName = villainInfo[0];

            //Да проверим дали града съществува
            string townId = EnsureTownExists(sqlConnection, minionTown, output);

            //Да проверим дали името на злодея съществува
            string villainId = EnsureVillainExists(sqlConnection, villainName, output);

            //Накрая трябва да довавим този минион в базата
            string insertMinionQueryText = @"INSERT INTO Minions(Name, Age, TownId)
                                                VALUES(@minionName, @minionAge, @townId)";
            using SqlCommand insertMinionCmd = new SqlCommand(insertMinionQueryText, sqlConnection);
            
            insertMinionCmd.Parameters.AddRange(new[]
            {
                new SqlParameter("@minionName", minionName),
                new SqlParameter("@minionAge", minionAge),
                new SqlParameter("@townId", townId),
            });

            insertMinionCmd.ExecuteNonQuery();

            string getMinionIdQueryText = @"SELECT Id FROM Minions WHERE Name = @minionName";

            using SqlCommand getMinionIdCmd = new SqlCommand(getMinionIdQueryText, sqlConnection);
            getMinionIdCmd.Parameters.AddWithValue("@minionName", minionName);

            string minionId = getMinionIdCmd.ExecuteScalar().ToString();

            string insertIntoMQueryTxt = @"INSERT INTO MinionsVillains(MinionId, VillainId)
                                            VALUES(@minionId, @villainId)";
            using SqlCommand insertIntoMappingCmd = new SqlCommand(insertIntoMQueryTxt, sqlConnection);
            insertIntoMappingCmd.Parameters.AddRange(new[]
            {
                new SqlParameter("@minionId", minionId),
                new SqlParameter("@villainId", villainId),

            });

            insertIntoMappingCmd.ExecuteNonQuery();

            output
                .AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

            return output.ToString().TrimEnd();
        }

        private static string EnsureVillainExists(SqlConnection sqlConnection, string villainName, StringBuilder output)
        {
            string getVillainIdQueryText = @"SELECT Id FROM Villains
                                        WHERE Name = @name";
            using SqlCommand getVillainIdCmd = new SqlCommand(getVillainIdQueryText, sqlConnection);
            getVillainIdCmd.Parameters.AddWithValue("@name", villainName);

            //Вземаме ID-то на злодея
            string villainId = getVillainIdCmd.ExecuteScalar()?.ToString();

            if(villainId == null)
            {
                string getFactorQueryText = @"SELECT Id FROM  EvilnessFactors WHERE Name = 'Evil'";

                using SqlCommand getFactorIdCmd = new SqlCommand(getFactorQueryText, sqlConnection);
                
                // Вземаме FactorId-to
                string factorId = getFactorIdCmd.ExecuteScalar()?.ToString();

                string insertVillainQueryText = @"INSERT INTO Villains(Name, EvilnessFactorId)
                                                    VALUES(@villainName, @factorId)";
                using SqlCommand insertVillainCmd = new SqlCommand(insertVillainQueryText, sqlConnection);
                insertVillainCmd.Parameters.AddWithValue("@villainName", villainName);

                insertVillainCmd.Parameters.AddWithValue("@factorId", factorId);

                insertVillainCmd.ExecuteNonQuery();

                villainId = getVillainIdCmd.ExecuteScalar().ToString();

                output
                    .AppendLine($"Villain {villainName} was added to the database.");
            }

            return villainId;
        }

        private static string EnsureTownExists(SqlConnection sqlConnection, string minionTown, StringBuilder output)
        {
            string getTownIdQueryText = @"SELECT Id FROM Towns
                                            WHERE Name = @townName";
            using SqlCommand getTownIdCmd = new SqlCommand(getTownIdQueryText, sqlConnection);
            getTownIdCmd.Parameters.AddWithValue("@townName", minionTown);

            //Вземаме ID-то на града
            string townId = getTownIdCmd.ExecuteScalar()?.ToString();

            //Ако не съществува такъв град
            if(townId == null)
            {
                string insertTownQueryText = @"INSERT INTO Towns(Name, CountryCode)
                                                VALUES(@townName, 1)";
                using SqlCommand insertTownCmd = new SqlCommand(insertTownQueryText, sqlConnection);
                insertTownCmd.Parameters.AddWithValue("@townName", minionTown);

                //Инсъртваме града
                insertTownCmd.ExecuteNonQuery();

                //Връщаме ID-то на инсъртнатия град
                townId = getTownIdCmd.ExecuteScalar().ToString();

                output
                    .AppendLine($"Town {minionTown} was added to the database.");
            }

            return townId;
        }
    }
}
