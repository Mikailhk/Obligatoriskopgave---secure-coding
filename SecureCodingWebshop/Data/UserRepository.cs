using Microsoft.Data.SqlClient;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }
        public void CreateUser(User user)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "INSERT INTO Users (Email, PasswordHash) VALUES (@Email, @PasswordHash)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public User? GetUserByEmail(string email)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "SELECT Id, Email, PasswordHash FROM Users WHERE Email = @Email";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    PasswordHash = reader.GetString(2)
                };
            }

            return null;
        }

        public void UpdatePassword(int userId, string newPasswordHash)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "UPDATE Users SET PasswordHash = @PasswordHash WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
            command.Parameters.AddWithValue("@Id", userId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
