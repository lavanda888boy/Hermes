using Microsoft.AspNetCore.Mvc;
using NotificationPreferencesService.Models;
using NotificationPreferencesService.Repository;

namespace NotificationPreferencesService.Controllers;

[ApiController]
[Route("/")]
public class NotificationPreferencesController : ControllerBase
{
    private readonly IRepository<NotificationPreference> _notificationPreferenceRepository;
    private readonly IRepository<DeviceTopicInfo> _deviceTopicInfoRepository;

    public NotificationPreferencesController(
        IRepository<NotificationPreference> notificationPreferenceRepository, 
        IRepository<DeviceTopicInfo> deviceTopicInfoRepository)
    {
        _notificationPreferenceRepository = notificationPreferenceRepository;
        _deviceTopicInfoRepository = deviceTopicInfoRepository;
    }

    [HttpGet]
    public async Task<List<string>> GetNotificationPreferenceOptions()
    {
        var preferences = await _notificationPreferenceRepository.GetFilteredAsync("Optional");
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

        return Ok(deviceToken);
    }

    [HttpPut("{deviceToken}")]
    public async Task<IActionResult> UpdateNotificationPreferences(string deviceToken, [FromBody] List<string> notificationPreferences)
    {
        var device = await _deviceTopicInfoRepository.GetByIdAsync(deviceToken);

        if (device == null)
        {
            return BadRequest($"Device with token {deviceToken} does not exist.");
        }

        device.SubscribedTopics = notificationPreferences;
        await _deviceTopicInfoRepository.UpdateAsync(device);

        return Ok(deviceToken);
    }
}
