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
        private readonly string _tableCreateQuery = "CREATE TABLE Products (Id, Name, Stock)";
        private readonly string _addItemQuery = "INSERT INTO Products (Id, Name, Stock)" +
                                                "VALUES (@id, @name, @stock)";
        private readonly string _selectFromIdQuery = "SELECT * FROM Products WHERE Id = @id";
        private readonly string _deleteFromIdQuery = "DELETE FROM Products WHERE Id = @id";
        private readonly string _fullUpdateFromIdQuery = "UPDATE Products SET Name = @name, Stock = @stock WHERE Id = @id";
        private readonly string _stockUpdateFromIdQuery = "UPDATE Products SET Stock = @stock WHERE Id = @id";
        private readonly string _nameUpdateFromIdQuery = "UPDATE Products SET Name = @name WHERE Id = @id";
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

            var id = product.Id;
            var name = product.Name;
            var stock = product.Stock;

            var addResult = connection.Execute(_addItemQuery, new { id, name, stock });

            return addResult != default;
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

            string query;
            if (product.Name != null && product.Stock != null)
            {
                query = _fullUpdateFromIdQuery;
            }
            else if (product.Name is null)
            {
                query = _stockUpdateFromIdQuery;
            }
            else
            {
                query = _nameUpdateFromIdQuery;
            }

            return UpdateItem(product, connection, query);
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

        private bool UpdateItem(Product product, SqliteConnection connection, string query)
        {
            var id = product.Id;
            var name = product.Name;
            var stock = product.Stock;

            var addResult = connection.Execute(query, new { id, name, stock });

            return addResult != default;
        }

        private SqliteConnection OpenConnection() => new SqliteConnection(_apiSettings.ConnectionString);
    }
}
