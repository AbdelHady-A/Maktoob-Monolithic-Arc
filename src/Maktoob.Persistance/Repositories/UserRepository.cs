using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Maktoob.Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MaktoobDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
