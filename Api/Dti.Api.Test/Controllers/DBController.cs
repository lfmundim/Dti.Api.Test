using System;
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
        /// <response code="StatusCodes.Status200OK">Item successfully added</response>
        /// <response code="StatusCodes.Status400BadRequest">Product ID already in use or request incomplete.</response>
        /// <response code="StatusCodes.Status500InternalServerError">Failed to add item</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

            return isSuccess ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
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
    }
}
