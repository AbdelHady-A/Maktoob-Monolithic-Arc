using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.Domain.Entities
{
    public class UserLogin : Entity<Guid>
    {
        public string RefreshToken { get; set; }
        public string JwtId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime RequiredUpdateDate { get; set; }
        public string OperatingSystem { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string UserAgent { get; set; }
        public string OperatingSystemVersion { get; set; }
        public Guid UserId { get; set; }
    }
}
