using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB implementation of IPropertyRepository.
    /// Handles data persistence and retrieval for properties.
    /// </summary>
    public class MongoPropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _collection;

        /// <summary>
        /// Initializes a new instance using the given settings.
        /// </summary>
        public MongoPropertyRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.Database);
            _collection = database.GetCollection<Property>(settings.Value.Collection);
        }

        /// <summary>
        /// Retrieves all properties matching the given optional filters.
        /// </summary>
        /// <param name="name">Optional name filter.</param>
        /// <param name="address">Optional address filter.</param>
        /// <param name="minPrice">Optional minimum price filter.</param>
        /// <param name="maxPrice">Optional maximum price filter.</param>
        /// <returns>List of matching properties.</returns>
        public async Task<IEnumerable<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                var filterBuilder = Builders<Property>.Filter;
                var filters = new List<FilterDefinition<Property>>();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    filters.Add(filterBuilder.Regex(nameof(Property.Name), new BsonRegularExpression(name, "i")));
                }

                if (!string.IsNullOrWhiteSpace(address))
                {
                    filters.Add(filterBuilder.Regex(nameof(Property.Address), new BsonRegularExpression(address, "i")));
                }

                if (minPrice.HasValue)
                {
                    filters.Add(filterBuilder.Gte(p => p.Price, minPrice.Value));
                }

                if (maxPrice.HasValue)
                {
                    filters.Add(filterBuilder.Lte(p => p.Price, maxPrice.Value));
                }

                var combinedFilter = filters.Count > 0
                    ? filterBuilder.And(filters)
                    : filterBuilder.Empty;

                return await _collection.Find(combinedFilter).ToListAsync();
            }
            catch (Exception ex)
            {
                // Consider logging here
                throw new Exception($"Error querying properties: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a property by its unique identifier.
        /// </summary>
        /// <param name="id">The property ID.</param>
        /// <returns>The matching property or null if not found.</returns>
        public async Task<Property?> GetByIdAsync(string id)
        {
            try
            {
                return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Adds a new property to the MongoDB collection.
        /// </summary>
        /// <param name="property">The property to add.</param>
        public async Task AddAsync(Property property)
        {
            try
            {
                await _collection.InsertOneAsync(property);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Deletes a property from the collection by its MongoDB ObjectId.
        /// </summary>
        /// <param name="id">The unique identifier of the property to delete.</param>
        /// <returns>True if a document was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
                var result = await _collection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting property with id '{id}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks whether a property with the given name and address already exists in the database.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="address">The address of the property.</param>
        /// <returns>
        /// A boolean indicating whether a matching property exists.
        /// Returns <c>true</c> if at least one property matches the name and address; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> ExistsAsync(string name, string address)
        {
            var filter = Builders<Property>.Filter.And(
                Builders<Property>.Filter.Eq(p => p.Name, name),
                Builders<Property>.Filter.Eq(p => p.Address, address)
            );

            return await _collection.Find(filter).AnyAsync();
        }
    }
}
