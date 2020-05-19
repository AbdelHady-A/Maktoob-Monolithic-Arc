using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Entities
{
    public interface IHasName
    {
        string Name { get; set; }
        string NormalizedName { get; set; }
    }
}
