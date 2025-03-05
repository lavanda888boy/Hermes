using MongoDB.Driver;
using NotificationPreferencesService.Models;

namespace NotificationPreferencesService.Repository
{
    public class NotificationPreferenceRepository : IRepository<NotificationPreference>
    {
        private readonly IMongoCollection<NotificationPreference> _notificationPreferencesCollection;

        public NotificationPreferenceRepository(IMongoDatabase mongoDatabase)
        {
            _notificationPreferencesCollection = mongoDatabase.GetCollection<NotificationPreference>("NotificationPreferences");
        }

        public async Task<List<NotificationPreference>> GetAllAsync()
        {
            var cursor = await _notificationPreferencesCollection.FindAsync(Builders<NotificationPreference>.Filter.Empty);
            var preferences = await cursor.ToListAsync();
            return preferences;
        }
    }
}
