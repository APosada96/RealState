namespace RealEstate.Infrastructure.Repositories
{
    /// <summary>
    /// Represents the MongoDB configuration options read from appsettings.json.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// MongoDB connection string.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Name of the MongoDB database.
        /// </summary>
        public string Database { get; set; } = string.Empty;

        /// <summary>
        /// Name of the collection for properties.
        /// </summary>
        public string Collection { get; set; } = string.Empty;
    }
}
