using Microsoft.Data.SqlClient;
using SecureCodingWebshop.Models;

namespace SecureCodingWebshop.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "SELECT Id, Name, Price FROM Products";

            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }

            return products;
        }

        public Product? GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "SELECT Id, Name, Price FROM Products WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                };
            }

            return null;
        }

        public List<Product> Search(string search)
        {
            List<Product> products = new List<Product>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "SELECT Id, Name, Price FROM Products WHERE Name LIKE @Search";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Search", "%" + search + "%");

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }

            return products;
        }

        public void Add(Product product)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "DELETE FROM Products WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Product product)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Id", product.Id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}