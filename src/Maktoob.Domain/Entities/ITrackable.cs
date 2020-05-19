using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Entities
{
    interface ITrackable
    {
        byte[] RowVersion { get; set; }
    }
}
