using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.CrossCuttingConcerns.Result;
using Maktoob.CrossCuttingConcerns.Security;
using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maktoob.Domain.Services
{
    public interface IUserService : ICrudService<User>
    {
        IPasswordHasher PasswordHasher { get; }
        IKeyNormalizer KeyNormalizer { get; }
    }
}
