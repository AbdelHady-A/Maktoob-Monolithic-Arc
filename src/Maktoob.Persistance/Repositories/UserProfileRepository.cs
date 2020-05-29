using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using MongoDB.Driver;

namespace Maktoob.Persistance.Repositories
{
    public class UserProfileRepository : MongoRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(IMongoDatabase database): base(database)
        {
        }
    }
}
