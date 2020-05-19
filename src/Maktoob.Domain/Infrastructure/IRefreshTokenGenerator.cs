using Maktoob.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Infrastructure
{
    public interface IRefreshTokenGenerator
    {
        string Generate(User user);
    }
}
