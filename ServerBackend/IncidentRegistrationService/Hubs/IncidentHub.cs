using IncidentRegistrationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IncidentRegistrationService.Hubs
{
    [Authorize]
    public class IncidentHub : Hub
    {
        public async Task SendMessage(Incident incident)
        {
            await Clients.All.SendAsync("ReceiveIncident", incident);
        }
    }
}
