using MongoDB.Bson;
using MongoDB.Driver;
using NotificationPreferencesService.Models;

namespace NotificationPreferencesService.Repository
{
    public class DeviceTopicInfoRepository : IRepository<DeviceTopicInfo>
    {
        private readonly IMongoCollection<DeviceTopicInfo> _deviceTopicInfoCollection;

        public DeviceTopicInfoRepository(IMongoDatabase mongoDatabase)
        {
            _deviceTopicInfoCollection = mongoDatabase.GetCollection<DeviceTopicInfo>("DeviceTopicInfo");
        }

        public async Task AddAsync(DeviceTopicInfo entity)
        {
            await _deviceTopicInfoCollection.InsertOneAsync(entity);
        }

        public async Task<List<DeviceTopicInfo>> GetAllAsync()
        {
            var cursor = await _deviceTopicInfoCollection.FindAsync(Builders<DeviceTopicInfo>.Filter.Empty);
            var deviceInfo = await cursor.ToListAsync();
            
            return deviceInfo;
        }

        public async Task<DeviceTopicInfo?> GetByIdAsync(string token)
        {
            var filter = Builders<DeviceTopicInfo>.Filter.Eq("DeviceId", token);
            var deviceCursor = await _deviceTopicInfoCollection.FindAsync(filter);
            var device = await deviceCursor.FirstOrDefaultAsync();
            
            return device;
        }

        public async Task UpdateAsync(DeviceTopicInfo entity)
        {
            var filter = Builders<DeviceTopicInfo>.Filter.Eq("_id", ObjectId.Parse(entity.Id));
            var update = Builders<DeviceTopicInfo>.Update.Set("SubscribedTopics", entity.SubscribedTopics);

            await _deviceTopicInfoCollection.UpdateOneAsync(filter, update);
        }
    }
}
