using System;

namespace Maktoob.CrossCuttingConcerns.Options
{
    public class JsonWebTokenOptions
    {

        public string Audience { get; set; }
        public string Algorithm { get; set; }
        public TimeSpan Expires { get; set; }
        public RefreshTokenOptions RefreshToken { get; set; }
        public string Issuer { get; set; }

        public string Key { get; set; }
    }

    public class RefreshTokenOptions
    {
        public TimeSpan Expires { get; set; }
        public TimeSpan UpdateRequired { get; set; }
    }
}
