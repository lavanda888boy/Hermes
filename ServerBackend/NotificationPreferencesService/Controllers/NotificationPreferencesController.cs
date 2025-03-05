using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NotificationPreferencesService.Models;
using NotificationPreferencesService.Repository;

namespace NotificationPreferencesService.Controllers;

[ApiController]
[Route("/")]
public class NotificationPreferencesController : ControllerBase
{
    private readonly FirebaseMessaging _firebaseMessaging;
    private readonly IRepository<NotificationPreference> _notificationPreferenceRepository;
    private readonly IRepository<DeviceTopicInfo> _deviceTopicInfoRepository;

    public NotificationPreferencesController(FirebaseApp firebaseApp, 
        IRepository<NotificationPreference> notificationPreferenceRepository, 
        IRepository<DeviceTopicInfo> deviceTopicInfoRepository)
    {
        _firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);
        _notificationPreferenceRepository = notificationPreferenceRepository;
        _deviceTopicInfoRepository = deviceTopicInfoRepository;
    }

    [HttpGet]
    public async Task<List<string>> GetNotificationPreferenceOptions()
    {
        var preferences = await _notificationPreferenceRepository.GetAllAsync();
        return [.. preferences.Select(p => p.PreferenceName)];
    }

    [HttpGet("{devicetoken}")]
    public async Task<IActionResult> GetNotificationPreferencesByDeviceToken(string deviceToken)
    {
        var device = await _deviceTopicInfoRepository.GetByIdAsync(deviceToken);

        if (device == null)
        {
            return BadRequest($"Device with token {deviceToken} does not exist.");
        }

        return Ok(device.SubscribedTopics);
    }

    [HttpPost("{deviceToken}")]
    public async Task<IActionResult> RegisterNotificationPreferences(string deviceToken, [FromBody] List<string> notificationPreferences)
    {
        var device = await _deviceTopicInfoRepository.GetByIdAsync(deviceToken);

        if (device != null)
        {
            return BadRequest($"Device with token {deviceToken} already exists.");
        }

        if (notificationPreferences == null || notificationPreferences.Count == 0)
        {
            return BadRequest("Notification preferences list for registration is empty");
        }

        await _deviceTopicInfoRepository.AddAsync(new DeviceTopicInfo { DeviceId = deviceToken, SubscribedTopics = notificationPreferences });
        var preferences = await _notificationPreferenceRepository.GetAllAsync();

        foreach (var preference in notificationPreferences)
        {
            var sanitizedTopic = preferences.First(p => p.PreferenceName == preference);
            await _firebaseMessaging.SubscribeToTopicAsync([deviceToken], sanitizedTopic.TopicName);
        }

        return Ok(deviceToken);
    }

    [HttpPut("{deviceToken}")]
    public async Task<IActionResult> UpdateNotificationPreferences(string deviceToken, [FromBody] List<string> notificationPreferences)
    {
        if (notificationPreferences == null || notificationPreferences.Count == 0)
        {
            return BadRequest("Notification preferences list for update is empty");
        }

        var device = await _deviceTopicInfoRepository.GetByIdAsync(deviceToken);

        if (device == null)
        {
            return BadRequest($"Device with token {deviceToken} does not exist.");
        }

        var topicsToSubscribe = notificationPreferences.Except(device.SubscribedTopics);
        var topicsToUnsubscribe = device.SubscribedTopics.Except(notificationPreferences);

        device.SubscribedTopics = [.. notificationPreferences.Intersect(device.SubscribedTopics)];
        await _deviceTopicInfoRepository.UpdateAsync(device);

        var preferences = await _notificationPreferenceRepository.GetAllAsync();

        foreach (var topic in topicsToSubscribe)
        {
            var sanitizedTopic = preferences.First(p => p.PreferenceName == topic);
            await _firebaseMessaging.SubscribeToTopicAsync([deviceToken], sanitizedTopic.TopicName);
        }

        foreach (var topic in topicsToUnsubscribe)
        {
            var sanitizedTopic = preferences.First(p => p.PreferenceName == topic);
            await _firebaseMessaging.UnsubscribeFromTopicAsync([deviceToken], sanitizedTopic.TopicName);
        }

        return Ok(deviceToken);
    }
}
