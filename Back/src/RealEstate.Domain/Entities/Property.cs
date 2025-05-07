using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities
{
    /// <summary>
    /// Represents a real estate property entity in the domain.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// MongoDB document ID (auto-generated).
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Identifier of the property's owner.
        /// </summary>
        [BsonElement("IdOwner")]
        public string IdOwner { get; set; } = string.Empty;

        /// <summary>
        /// Name or title of the property.
        /// </summary>

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Physical address of the property.
        /// </summary>
        [BsonElement("Address")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Sale price of the property in local currency.
        /// </summary>

        [BsonElement("Price")]
        public decimal Price { get; set; }

        /// <summary>
        /// URL pointing to an image of the property.
        /// </summary>

        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
