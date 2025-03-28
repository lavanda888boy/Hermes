using IncidentRegistrationService.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IncidentRegistrationService.Repository
{
    public class IncidentRepository : IRepository<Incident>
    {
        private readonly IMongoCollection<Incident> _incidentCollection;

        public IncidentRepository(IMongoDatabase mongoDatabase)
        {
            _incidentCollection = mongoDatabase.GetCollection<Incident>("Incidents");
        }

        public async Task AddAsync(Incident entity)
        {
            await _incidentCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Incident entity)
        {
            var filter = Builders<Incident>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
            await _incidentCollection.DeleteOneAsync(filter);
        }

        public async Task<List<Incident>> GetAllAsync()
        {
            var incidentCursor = await _incidentCollection.FindAsync(Builders<Incident>.Filter.Empty);
            var incidents = await incidentCursor.ToListAsync();

            return incidents;
        }

        public async Task<Incident?> GetByIdAsync(string id)
        {
            var filter = Builders<Incident>.Filter.Eq("_id", ObjectId.Parse(id));
            var incidentCursor = await _incidentCollection.FindAsync(filter);
            var incident = await incidentCursor.FirstOrDefaultAsync();

            return incident;
        }

        public async Task<List<Incident>> GetFilteredAsync()
        {
            var filter = Builders<Incident>.Filter.Eq("Status", "pending");
            var incidentCursor = await _incidentCollection.FindAsync(filter);
            var filteredIncidents = await incidentCursor.ToListAsync();

            return filteredIncidents;
        }

        public async Task UpdateAsync(Incident entity)
        {
            var filter = Builders<Incident>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
            await _incidentCollection.ReplaceOneAsync(filter, entity);
        }
    }
}
