using IncidentRegistrationService.Models;
using MongoDB.Driver;

namespace IncidentRegistrationService.Repository
{
    public class NotificationPreferenceRepository : IRepository<NotificationPreference>
    {
        private readonly IMongoCollection<NotificationPreference> _notificationPreferencesCollection;

        public NotificationPreferenceRepository(IMongoDatabase mongoDatabase)
        {
            _notificationPreferencesCollection = mongoDatabase.GetCollection<NotificationPreference>("NotificationPreferences");
        }

        public async Task<List<NotificationPreference>> GetFilteredAsync(string flag)
        {
            var filter = Builders<NotificationPreference>.Filter.Eq("ImportanceFlag", flag);
            var preferencesCursor = await _notificationPreferencesCollection.FindAsync(filter);
            var preferences = await preferencesCursor.ToListAsync();

            return preferences;
        }
    }
}
