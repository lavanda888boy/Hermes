using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using NotificationPreferencesService.Models;

namespace NotificationPreferencesService.Controllers;

[ApiController]
[Route("/")]
public class NotificationPreferencesController : ControllerBase
{
    private readonly FirebaseMessaging _firebaseMessaging;

    public NotificationPreferencesController(FirebaseApp firebaseApp)
    {
        _firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);
    }

    [HttpGet]
    public List<string?> GetNotificationPreferencesOptions()
    {
        return [.. typeof(NotificationPreferencesOption)
                    .GetFields()
                    .Select(f => f.GetValue(null)?.ToString())];
    }

    [HttpPost("{deviceToken}")]
    public async Task<IActionResult> RegisterNotificationPreferences(string deviceToken, [FromBody] List<string> notificationPreferences)
    {
        if (notificationPreferences == null || notificationPreferences.Count == 0)
        {
            return BadRequest("Notification preferences list for registration is empty");
        }

        List<string> validTopics = [.. typeof(NotificationPreferencesOption)
                                         .GetFields()
                                         .Select(f => f.GetValue(null)?.ToString())];

        foreach (var topic in notificationPreferences)
        {
            if (validTopics.Contains(topic))
            {
                try
                {
                    await _firebaseMessaging.SubscribeToTopicAsync([deviceToken], SanitizeTopic(topic));
                }
                catch (FirebaseMessagingException ex)
                {
                    Console.WriteLine($"Message: {ex.Message};\nTopic: {topic}");
                }
            }
            else
            {
                Console.WriteLine($"Subscription topic is not available: {topic}");
            }
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

        List<string> validTopics = [.. typeof(NotificationPreferencesOption)
                                         .GetFields()
                                         .Select(f => f.GetValue(null)?.ToString())];

        var topicsToUnsubscribe = validTopics.Except(notificationPreferences);

        foreach (var topic in topicsToUnsubscribe)
        {
            try
            {
                await _firebaseMessaging.UnsubscribeFromTopicAsync([deviceToken], SanitizeTopic(topic));
            }
            catch (FirebaseMessagingException ex)
            {
                Console.WriteLine($"Message: {ex.Message};\nTopic: {topic}");
            }
        }

        foreach (var topic in notificationPreferences)
        {
            if (validTopics.Contains(topic))
            {
                try
                {
                    await _firebaseMessaging.SubscribeToTopicAsync([deviceToken], SanitizeTopic(topic));
                }
                catch (FirebaseMessagingException ex)
                {
                    Console.WriteLine($"Message: {ex.Message};\nTopic: {topic}");
                }
            }
            else
            {
                Console.WriteLine($"Subscription topic is not available: {topic}");
            }
        }

        return Ok(deviceToken);
    }

    private static string SanitizeTopic(string topic)
    {
        return topic.Replace("&", "").Replace(" ", "_").ToLower();
    }
}
