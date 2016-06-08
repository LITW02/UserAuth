using System.Data.SqlClient;

namespace UserAuth.Data
{
    public class UserManager
    {
        private string _connectionString;

        public UserManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(string username, string password)
        {
            User user = new User();
            user.Username = username;
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Users (Username, PasswordHash, PasswordSalt) VALUES " +
                                      "(@username, @password, @salt)";
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.PasswordHash);
                command.Parameters.AddWithValue("@salt", user.PasswordSalt);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public User Login(string username, string password)
        {
            User user = GetUser(username);
            if (user == null)
            {
                return null;
            }

            bool isMatch = PasswordHelper.IsMatch(password, user.PasswordHash, user.PasswordSalt);
            if (isMatch)
            {
                return user;
            }

            return null;
        }

        private User GetUser(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users WHERE Username = @username";
                command.Parameters.AddWithValue("@username", username);
                connection.Open();
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }

                User user = new User();
                user.Id = (int) reader["Id"];
                user.Username = (string)reader["Username"];
                user.PasswordHash = (string)reader["PasswordHash"];
                user.PasswordSalt = (string)reader["PasswordSalt"];
                return user;

            }
        }

    }
}