using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using StackExchange.Redis;

namespace IncidentRegistrationService.Services
{
    public class IncidentCorrelationService : IIncidentCorrelationService
    {
        private readonly IDatabase _redisGPSStorage;
        private readonly string RedisGeoKey = "device_locations";

        private readonly IRepository<Incident> _incidentRepository;

        public IncidentCorrelationService(IConnectionMultiplexer redisResource, 
            IRepository<Incident> incidentRepository)
        {
            _redisGPSStorage = redisResource.GetDatabase();
            _incidentRepository = incidentRepository;
        }

        public async Task<bool> CheckIncidentDuplicate(Incident incident)
        {
            var incidents = await _incidentRepository.GetAllAsync();
            var similarIncidents = incidents.Where(i => i.Category == incident.Category).ToList();

            if (similarIncidents.Count == 0)
            {
                return false;
            }

            foreach (var inc in similarIncidents)
            {
                var result = await _redisGPSStorage.GeoRadiusAsync(
                    RedisGeoKey,
                    inc.Longitude, 
                    inc.Latitude,
                    inc.AreaRadius,
                    GeoUnit.Kilometers);

                if (result.Any(r => r.Member.ToString() == incident.UserToReport) &&
                    incident.Timestamp < inc.Timestamp.AddMinutes(30))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
