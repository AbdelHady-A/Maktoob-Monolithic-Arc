using Maktoob.Domain.Entities;
using Maktoob.Domain.Repositories;
using Maktoob.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Persistance.Repositories
{
    public class UserLoginRepository : Repository<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(MaktoobDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }
    }
}
