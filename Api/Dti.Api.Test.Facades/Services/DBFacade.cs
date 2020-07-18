using System;
using System.Collections.Generic;
using System.Linq;

using Dapper;

using Dti.Api.Test.Models.Database;
using Dti.Api.Test.Models.UI;

using Microsoft.Data.Sqlite;

namespace Dti.Api.Test.Facades.Services
{
    public class DBFacade : IDBFacade
    {
        private readonly ApiSettings _apiSettings;
        private readonly string _tableCreateQuery = "CREATE TABLE Products (Id, Name, Stock, Price)";
        private readonly string _addItemQuery = "INSERT INTO Products (Id, Name, Stock, Price)" +
                                                "VALUES (@id, @name, @stock, @price)";
        private readonly string _selectFromIdQuery = "SELECT * FROM Products WHERE Id = @id";
        private readonly string _deleteFromIdQuery = "DELETE FROM Products WHERE Id = @id";
        private readonly string _baseUpdateFromIdQuery = "UPDATE Products SET {0} WHERE Id = @id";
        private readonly string _selectAllQuery = "SELECT * FROM Products";

        public DBFacade(ApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public bool AddNewItem(Product product)
        {
            using var connection = OpenConnection();

            if (ProductExists(product, connection))
            {
                throw new ArgumentException("Product ID already registered. Please provide a new one.", nameof(product.Id));
            }
            if (!product.IsComplete())
            {
                throw new ArgumentException("Product name or ID invalid. IDs must be greater than zero.");
            }

            return AddOrUpdateItem(product, connection, _addItemQuery);
        }

        public bool DeleteItem(long id)
        {
            using var connection = OpenConnection();

            var deleteResult = connection.Execute(_deleteFromIdQuery, new { id });

            return deleteResult != default;
        }

        public bool UpdateItem(Product product)
        {
            using var connection = OpenConnection();

            if (!ProductExists(product, connection))
            {
                throw new ArgumentException($"Product with ID {product.Id} does not exist.", nameof(product.Id));
            }

            var propertyList = new List<string>();
            if (product.Name != null)
            {
                propertyList.Add("Name = @name");
            }
            if (product.Price != null)
            {
                propertyList.Add("Price = @price");
            }
            if(product.Stock != null)
            {
                propertyList.Add("Stock = @stock");
            }

            var properties = propertyList.Aggregate((current, next) => $"{current}, {next}");
            return AddOrUpdateItem(product, connection, string.Format(_baseUpdateFromIdQuery, properties));
        }

        public Product GetItem(long id)
        {
            using var connection = OpenConnection();

            return GetProduct(connection, id);
        }

        public IEnumerable<Product> GetAllItems()
        {
            using var connection = OpenConnection();

            return connection.Query<Product>(_selectAllQuery);
        }

        public bool CreateDatabase()
        {
            using var connection = OpenConnection();

            connection.Open();
            var affectedRows = connection.Execute(_tableCreateQuery);

            return true;
        }

        private bool ProductExists(Product product, SqliteConnection connection)
        {
            var id = product.Id;
            var existingProduct = GetProduct(connection, id);
            return existingProduct != null;
        }

        private Product GetProduct(SqliteConnection connection, long id)
        {
            return connection.Query<Product>(_selectFromIdQuery, new { id })
                                                        .FirstOrDefault();
        }

        private bool AddOrUpdateItem(Product product, SqliteConnection connection, string query)
        {
            var id = product.Id;
            var name = product.Name;
            var stock = product.Stock;
            var price = product.Price;

            var result = connection.Execute(query, new { id, name, stock, price });

            return result != default;
        }

        private SqliteConnection OpenConnection() => new SqliteConnection(_apiSettings.ConnectionString);
    }
}
