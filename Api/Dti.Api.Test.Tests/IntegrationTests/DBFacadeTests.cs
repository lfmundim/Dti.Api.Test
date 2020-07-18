using System;
using System.IO;
using System.Linq;

using Dti.Api.Test.Facades;
using Dti.Api.Test.Facades.Services;
using Dti.Api.Test.Models.Database;
using Dti.Api.Test.Models.UI;

using Shouldly;

using Xunit;

namespace Dti.Api.Test.Tests.IntegrationTests
{
    public class DBFacadeTests
    {
        private const long ID = long.MaxValue;

        // Global Set-up
        private readonly ApiSettings _settings;
        private readonly IDBFacade _facade;

        public DBFacadeTests()
        {
            var filePath = $"{Environment.CurrentDirectory}\\db.sqlite";

            _settings = new ApiSettings
            {
                ConnectionString = $"Data Source={filePath}"
            };
            _facade = new DBFacade(_settings);

            var isFirstRun = !File.Exists(filePath);
            if (isFirstRun)
            {
                _facade.CreateDatabase();
            }
        }

        [Fact]
        public void AddAndRemoveItem_Test()
        {
            // Arrange
            var product = CreateProduct();

            // Act
            var addResponse = _facade.AddNewItem(product);
            var removeResponse = _facade.DeleteItem(product.Id);

            // Assert
            addResponse.ShouldBeTrue();
            removeResponse.ShouldBeTrue();
        }

        [Fact]
        public void AddInvalidId_Test()
        {
            // Arrange
            var product = CreateProduct(default);
            var exceptionThrown = false;

            // Act
            try
            {
                var addResponse = _facade.AddNewItem(product);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            exceptionThrown.ShouldBeTrue();
        }

        [Fact]
        public void AddDuplicateId_Test()
        {
            // Arrange
            var product = CreateProduct();
            var exceptionThrown = false;
            var addResponse = _facade.AddNewItem(product);

            // Act
            try
            {
                var secondAddResponse = _facade.AddNewItem(product);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }
            var removeResponse = _facade.DeleteItem(product.Id);

            // Assert

            exceptionThrown.ShouldBeTrue();
            removeResponse.ShouldBeTrue();
            addResponse.ShouldBeTrue();
        }

        [Theory]
        [InlineData("newName", 42, 42.5)]
        [InlineData(null, 42, 42.5)]
        [InlineData("newName", null, 42.5)]
        [InlineData("newName", 42, null)]
        [InlineData("newName", null, null)]
        [InlineData(null, 42, null)]
        [InlineData(null, null, 42.5)]
        public void UpdateAndGetItem_Test(string newName, int? newStock, double? newPrice)
        {
            // Arrange
            var product = CreateProduct();
            var addResponse = _facade.AddNewItem(product);
            var updatedProduct = CreateProduct(newName, newStock);

            // Act
            var updateResponse = _facade.UpdateItem(updatedProduct);
            var updatedDbProduct = _facade.GetItem(ID);
            var deleteResponse = _facade.DeleteItem(ID);

            // Assert
            updateResponse.ShouldBeTrue();
            addResponse.ShouldBeTrue();
            updatedDbProduct.Id.ShouldBe(ID);
            updatedDbProduct.Stock.ShouldBe(newStock is null ? product.Stock : newStock);
            updatedDbProduct.Name.ShouldBe(newName is null ? product.Name : newName);
            updatedDbProduct.Price.ShouldBe(newPrice is null ? product.Price : newPrice);
            deleteResponse.ShouldBeTrue();
        }

        [Fact]
        public void GetItem_Test()
        {
            // Arrange
            var product = CreateProduct();
            var addResponse = _facade.AddNewItem(product);

            // Act
            var retrievedItem = _facade.GetItem(ID);
            var deleteResponse = _facade.DeleteItem(ID);

            // Assert
            addResponse.ShouldBeTrue();
            deleteResponse.ShouldBeTrue();
            retrievedItem.ShouldBe(product);
        }

        [Fact]
        public void GetAllItems_Test()
        {
            // Arrange
            var product = CreateProduct();
            var addResponse = _facade.AddNewItem(product);

            // Act
            var retrievedItems = _facade.GetAllItems();
            var deleteResponse = _facade.DeleteItem(ID);

            // Assert
            addResponse.ShouldBeTrue();
            deleteResponse.ShouldBeTrue();
            retrievedItems.Any().ShouldBeTrue();
        }

        private static Product CreateProduct(string name = "integrationTest", int? stock = 42, double? price = 42.5, long id = ID)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Stock = stock,
                Price = price
            };
        }
    }
}
