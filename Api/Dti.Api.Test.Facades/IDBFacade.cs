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
        void UpdateItem(Product product);

        /// <summary>
        /// Deletes an item from the DB
        /// </summary>
        /// <param name="product">Item to be deleted</param>
        void DeleteItem(Product product);

        /// <summary>
        /// Deletes an item from the DB
        /// </summary>
        /// <param name="productId">ID of the item to be deleted</param>
        void DeleteItem(long productId);

        /// <summary>
        /// Kickstarts the Database
        /// </summary>
        bool CreateDatabase();
    }
}
