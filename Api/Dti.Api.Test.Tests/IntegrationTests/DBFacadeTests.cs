using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task AddAndRemoveItem_TestAsync()
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
        public async Task AddInvalidId_TestAsync()
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
        public async Task AddDuplicateId_TestAsync()
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
        [InlineData("newName", 42)]
        [InlineData(null, 42)]
        [InlineData("newName", null)]
        public async Task UpdateAndGetItem_TestAsync(string newName, int? newStock)
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
            deleteResponse.ShouldBeTrue();
        }

        [Fact]
        public async Task GetItem_TestAsync()
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
        public async Task GetAllItems_TestAsync()
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

        private static Product CreateProduct(string name = "integrationTest", int? stock = 42, long id = ID)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Stock = stock
            };
        }
    }
}
