using System;
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
        private readonly string _selectFirstFromIdQuery = "SELECT * FROM Products WHERE Id = @id";

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

            var id = product.Id;
            var name = product.Name;
            var stock = product.Stock;

            var addResult = connection.Execute(_addItemQuery, new { id, name, stock });

            return addResult != default;
        }

        public bool CreateDatabase()
        {
            using var connection = OpenConnection();

            connection.Open();
            var affectedRows = connection.Execute(_tableCreateQuery);

            return true;
        }

        public void DeleteItem(Product product)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(long productId)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Product product)
        {
            throw new NotImplementedException();
        }

        private SqliteConnection OpenConnection() => new SqliteConnection(_apiSettings.ConnectionString);

        private bool ProductExists(Product product, SqliteConnection connection)
        {
            var id = product.Id;
            var existingProduct = connection.Query<Product>(_selectFirstFromIdQuery, new { id })
                                            .FirstOrDefault();
            return existingProduct != null;
        }
    }
}
