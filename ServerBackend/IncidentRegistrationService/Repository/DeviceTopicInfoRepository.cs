using MongoDB.Driver;
using IncidentRegistrationService.Models;

namespace IncidentRegistrationService.Repository
{
    public class DeviceTopicInfoRepository : IRepository<DeviceTopicInfo>
    {
        private readonly IMongoCollection<DeviceTopicInfo> _deviceTopicInfoCollection;

        public DeviceTopicInfoRepository(IMongoDatabase mongoDatabase)
        {
            _deviceTopicInfoCollection = mongoDatabase.GetCollection<DeviceTopicInfo>("DeviceTopicInfo");
        }

        public async Task<List<DeviceTopicInfo>> GetAllAsync()
        {
            var cursor = await _deviceTopicInfoCollection.FindAsync(Builders<DeviceTopicInfo>.Filter.Empty);
            var deviceInfo = await cursor.ToListAsync();

            return deviceInfo;
        }
    }
}
