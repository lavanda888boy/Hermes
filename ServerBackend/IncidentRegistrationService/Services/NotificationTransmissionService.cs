using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Models;

namespace IncidentRegistrationService.Services
{
    public class NotificationTransmissionService : INotificationTransmissionService
    {
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly IRepository<DeviceTopicInfo> _deviceTopicInfoRepository;
        private readonly IRepository<NotificationPreference> _notificationPreferenceRepository;

        public NotificationTransmissionService(FirebaseApp firebaseApp,
            IRepository<DeviceTopicInfo> deviceTopicInfoRepository,
            IRepository<NotificationPreference> notificationPreferenceRepository)
        {
            _firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);
            _deviceTopicInfoRepository = deviceTopicInfoRepository;
            _notificationPreferenceRepository = notificationPreferenceRepository;
        }

        public async Task SendIncidentNotification(Dictionary<string, string> data)
        {
            var deviceTokens = await GetDeviceTokensOfAffectedUsers(data["Category"]);

            foreach (var deviceToken in deviceTokens)
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Data = data,
                };

                await _firebaseMessaging.SendAsync(message);
            }
        }

        private async Task<List<string>> GetDeviceTokensOfAffectedUsers(string incidentCategory)
        {
            var deviceTokens = new List<string>();

            var mandatoryCategories = await _notificationPreferenceRepository.GetFilteredAsync("Mandatory");
            var matchingPreference = mandatoryCategories.FirstOrDefault(p => p.PreferenceName == incidentCategory);

            var devices = await _deviceTopicInfoRepository.GetAllAsync();

            if (matchingPreference != null)
            {
                return [.. devices.Select(d => d.DeviceId)];
            }

            return [.. devices.Where(d => d.SubscribedTopics.Contains(incidentCategory)).Select(d => d.DeviceId)];
        }
    }
}
