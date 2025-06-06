﻿using MongoDB.Driver;
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
            var preferencesCursor = await _notificationPreferencesCollection.FindAsync(Builders<NotificationPreference>.Filter.Empty);
            var preferences = await preferencesCursor.ToListAsync();
            
            return preferences;
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
