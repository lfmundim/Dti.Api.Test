using System.Collections.Generic;
using Dti.Api.Test.Models.Database;

namespace Dti.Api.Test.Facades
{
    /// <summary>
    /// Provides easy to use methods regarding DataBase usage
    /// </summary>
    public interface IDBFacade
    {
        /// <summary>
        /// Adds a new item to the DB
        /// </summary>
        /// <param name="product">Item to be added, including stock</param>
        /// <returns>bool indicating if the product was added</returns>
        /// <exception cref="ArgumentException">Thrown if the provided ID is already in use</exception>
        bool AddNewItem(Product product);

        /// <summary>
        /// Updates an item's stock and/or name
        /// </summary>
        /// <param name="product">Item to be updated, with the new values</param>
        /// <returns>bool indicating if the product was updated</returns>
        bool UpdateItem(Product product);

        /// <summary>
        /// Deletes an item from the DB
        /// </summary>
        /// <param name="productId">ID of the item to be deleted</param>
        /// <returns>bool indicating if the product was deleted</returns>
        bool DeleteItem(long productId);

        /// <summary>
        /// Retrieves an item from the DB
        /// </summary>
        /// <param name="id">ID of the item to be retrieved</param>
        /// <returns>The current representation of the item on the DB</returns>
        Product GetItem(long id);

        /// <summary>
        /// Retrieves all items from the DB
        /// </summary>
        /// <returns>Current representation of all items on the DB</returns>
        IEnumerable<Product> GetAllItems();

        /// <summary>
        /// Kickstarts the Database
        /// </summary>
        bool CreateDatabase();
    }
}
