using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Maktoob.Domain.Infrastructure
{
    public interface IJsonWebTokenCoder
    {
        Dictionary<string, object> Decode(string token);
        string Encode(IEnumerable<Claim> claims);
    }
}
