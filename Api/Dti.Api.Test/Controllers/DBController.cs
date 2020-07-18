using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dti.Api.Test.Facades;
using Dti.Api.Test.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dti.Api.Test.Controllers
{
    /// <summary>
    /// Controller responsible for direct DB interface
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class DBController : ControllerBase
    {
        private readonly IDBFacade _dBFacade;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DBController(IDBFacade dBFacade)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _dBFacade = dBFacade;
        }

        /// <summary>
        /// Attempts to add a new product to the DataBase
        /// </summary>
        /// <param name="product">Must provide the entire product object</param>
        /// <response code="201">Item successfully added</response>
        /// <response code="400">Product ID already in use or request incomplete.</response>
        /// <response code="500">Failed to add item</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddNewItem([FromBody] Product product)
        {
            var isRequestValid = product.IsComplete();
            if (!isRequestValid)
            {
                throw new ArgumentException("Request object incomplete. Please provide Product ID and Name.");
            }

            var isSuccess = _dBFacade.AddNewItem(product);
            var statusCode = isSuccess ? StatusCodes.Status201Created : StatusCodes.Status500InternalServerError;

            return StatusCode(statusCode);
        }

        /// <summary>
        /// Attempts to delete a product from the DataBase
        /// </summary>
        /// <param name="productId">The id of the product to be deleted</param>
        /// <response code="200">Item successfully deleted</response>
        /// <response code="400">Product with ID does not exist.</response>
        /// <response code="500">Failed to add item</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem([FromQuery(Name = "id"), Required] long productId)
        {
            var isSuccess = _dBFacade.DeleteItem(productId);
            return OkOrBadRequest(isSuccess);
        }

        /// <summary>
        /// Attempts to update a product on the DataBase
        /// </summary>
        /// <param name="product">The full representation of the new state of the product. <c>Id</c> will be used to match only</param>
        /// <response code="200">Item successfully updated</response>
        /// <response code="400">Product with ID does not exist.</response>
        /// <response code="500">Failed to update item</response>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateItem([FromBody] Product product)
        {
            var isSuccess = _dBFacade.UpdateItem(product);
            return OkOrBadRequest(isSuccess);
        }

        /// <summary>
        /// Attempts to retrieve an item's representation from the DataBase
        /// </summary>
        /// <param name="id">ID of the item to be retrieved</param>
        /// <response code="200">Item retrieved</response>
        /// <response code="204">No representation for the ID was found.</response>
        /// <response code="500">Failed to retrieve item</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItem([FromRoute(Name = "id"), Required] long id)
        {
            var item = _dBFacade.GetItem(id);

            return item is null ? NoContent() : (IActionResult)Ok(item);
        }

        /// <summary>
        /// Attempts to retrieve all item representations from the DataBase
        /// </summary>
        /// <response code="200">Items retrieved</response>
        /// <response code="204">No items were found.</response>
        /// <response code="500">Failed to retrieve items</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllItems()
        {
            var response = _dBFacade.GetAllItems();

            return response.Any() ? (IActionResult)Ok(response) : NoContent();
        }

        /// <summary>
        /// Kickstarts the DB
        /// </summary>
        [HttpPost("initialize")]
        public IActionResult CreateDatabase()
        {
            var response = _dBFacade.CreateDatabase();

            return response ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }


        private IActionResult OkOrBadRequest(bool isSuccess)
        {
            return isSuccess ? Ok() : StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
