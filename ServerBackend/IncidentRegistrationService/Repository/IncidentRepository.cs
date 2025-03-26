using IncidentRegistrationService.Models;
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

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Incident>> GetAllAsync()
        {
            var incidentCursor = await _incidentCollection.FindAsync(Builders<Incident>.Filter.Empty);
            var incidents = await incidentCursor.ToListAsync();

            return incidents;
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
            throw new NotImplementedException();
        }
    }
}
