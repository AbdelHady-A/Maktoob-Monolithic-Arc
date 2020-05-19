using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Normalizers
{
    public class NameNormalizer : IKeyNormalizer
    {
        public string Normalize(string key)
            => key.Normalize().ToUpperInvariant();
    }
}
