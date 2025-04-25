using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Models;
using StackExchange.Redis;

namespace IncidentRegistrationService.Services
{
    public class NotificationTransmissionService : INotificationTransmissionService
    {
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly IDatabase _redisGPSStorage;
        private readonly string RedisGeoKey = "device_locations";

        private readonly IRepository<DeviceTopicInfo> _deviceTopicInfoRepository;
        private readonly IRepository<NotificationPreference> _notificationPreferenceRepository;

        public NotificationTransmissionService(FirebaseApp firebaseApp,
            IConnectionMultiplexer redisResource,
            IRepository<DeviceTopicInfo> deviceTopicInfoRepository,
            IRepository<NotificationPreference> notificationPreferenceRepository)
        {
            _firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);
            _redisGPSStorage = redisResource.GetDatabase();
            _deviceTopicInfoRepository = deviceTopicInfoRepository;
            _notificationPreferenceRepository = notificationPreferenceRepository;
        }

        public async Task SendIncidentNotification(Incident incident, string note = "")
        {
            var deviceTokens = await GetDeviceTokensOfAffectedUsers(incident);

            foreach (var deviceToken in deviceTokens)
            {
                var notificationData = new Dictionary<string, string>()
                {
                    { "Category", incident.Category },
                    { "AreaRadius", incident.AreaRadius.ToString() },
                    { "Severity", Enum.GetName(typeof(IncidentSeverity), incident.Severity) },
                    { "Description", incident.Description },
                    { "Note", note }
                };

                var message = new Message()
                {
                    Token = deviceToken,
                    Data = notificationData,
                };

                await _firebaseMessaging.SendAsync(message);
            }
        }

        private async Task<List<string>> GetDeviceTokensOfAffectedUsers(Incident incident)
        {
            var devices = await _deviceTopicInfoRepository.GetAllAsync();
            devices = [.. devices.Where(d => d.DeviceId != incident.UserToReport)];
            
            var filteredDevices = await FilterDevicesByIncidentCategory(devices, incident.Category);
            var filteredTokens = filteredDevices.Select(fd => fd.DeviceId).ToList();

            filteredTokens = await FilterDevicesByGPSLocation(filteredTokens, incident);
            return filteredTokens;
        }

        private async Task<List<DeviceTopicInfo>> FilterDevicesByIncidentCategory(List<DeviceTopicInfo> devices, string incidentCategory)
        {
            var mandatoryCategories = await _notificationPreferenceRepository.GetFilteredAsync("Mandatory");
            var matchingPreference = mandatoryCategories.FirstOrDefault(p => p.PreferenceName == incidentCategory);

            if (matchingPreference != null)
            {
                return devices;
            }

            return [.. devices.Where(d => d.SubscribedTopics.Contains(incidentCategory))];
        }

        private async Task<List<string>> FilterDevicesByGPSLocation(List<string> filteredTokens, Incident incident)
        {
            var devicesWithinTheRadius = await _redisGPSStorage.GeoRadiusAsync(
                RedisGeoKey, 
                incident.Longitude, 
                incident.Latitude, 
                incident.AreaRadius, 
                GeoUnit.Kilometers);

            var deviceTokens = devicesWithinTheRadius.Select(d => d.Member.ToString()).ToList();

            return [.. deviceTokens.Intersect(filteredTokens)];
        }
    }
}
