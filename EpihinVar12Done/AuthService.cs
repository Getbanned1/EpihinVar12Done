using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpihinVar12
{
    public class AuthService
    {
        public long checkkForLogin(string username, string password)
        {
            using (NpgsqlConnection con = GetConnection())
            {
                con.Open();
                string query = "SELECT COUNT(id) FROM Users WHERE Login = @login AND Password = @password;";
                using (var command = new NpgsqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("login", username);
                    command.Parameters.AddWithValue("password", password);
                    var count = (long)command.ExecuteScalar();
                    return count; // Return count of matched rows (should be 1 if successful, 0 if failed)
                }
            }
        }

        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;UserId=postgres;Password=12345;Database=Var12DB");
        }
    }

}
