using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
