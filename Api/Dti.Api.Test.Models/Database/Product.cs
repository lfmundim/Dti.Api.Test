﻿namespace Dti.Api.Test.Models.Database
{
    /// <summary>
    /// Represents the entirety of a product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// The unique identifier for the product within the DB
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Non-unique readable product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// How many products are currently in stock
        /// </summary>
        public int? Stock { get; set; }

        /// <summary>
        /// Price per unit of product
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// Checks if ID and Name aren't default values to see
        /// if the instance is complete
        /// </summary>
        /// <returns><c>true</c> if Name and ID are not default. <c>false</c> otherwise</returns>
        public bool IsComplete() => !string.IsNullOrWhiteSpace(Name) && Id != default;

        public override bool Equals(object obj)
        {
            return obj is Product comparer
                   && comparer.Id.Equals(Id)
                   && comparer.Name.Equals(Name)
                   && comparer.Stock.Equals(Stock)
                   && comparer.Price.Equals(Price);
        }
    }
}
