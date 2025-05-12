using AdminAuthenticationService.Models;
using MongoDB.Driver;

namespace AdminAuthenticationService.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _userCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task AddAsync(User entity)
        {
            await _userCollection.InsertOneAsync(entity);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            var filter = Builders<User>.Filter.Eq("UserName", userName);
            var userCursor = await _userCollection.FindAsync(filter);
            var user = await userCursor.FirstOrDefaultAsync();

            return user;
        }
    }
}
