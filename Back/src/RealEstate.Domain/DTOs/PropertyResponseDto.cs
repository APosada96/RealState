namespace RealEstate.Domain.DTOs
{
    /// <summary>
    /// Represents the data returned to clients for a property.
    /// </summary>
    public class PropertyResponseDto
    {
        /// <summary>
        /// Unique identifier of the property.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the property's owner.
        /// </summary>
        public string IdOwner { get; set; } = string.Empty;

        /// <summary>
        /// Name or title of the property.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Physical address of the property.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Sale price of the property in local currency.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL pointing to an image of the property.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }
}
