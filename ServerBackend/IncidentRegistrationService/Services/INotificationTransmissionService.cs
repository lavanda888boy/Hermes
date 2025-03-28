namespace IncidentRegistrationService.Services
{
    public interface INotificationTransmissionService
    {
        Task SendIncidentNotification(Dictionary<string, string> data);
    }
}
