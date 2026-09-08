using Microsoft.Data.SqlClient;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Data
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public List<Order> GetOrdersByEmail(string email)
        {
            List<Order> orders = new();

            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "SELECT Id, UserEmail, TotalPrice, OrderDate FROM Orders WHERE UserEmail = @Email";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(new Order
                {
                    Id = reader.GetInt32(0),
                    UserEmail = reader.GetString(1),
                    TotalPrice = reader.GetDecimal(2),
                    OrderDate = reader.GetDateTime(3)
                });
            }

            return orders;
        }
    }
}