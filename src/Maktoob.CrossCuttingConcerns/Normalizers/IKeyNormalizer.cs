using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Normalizers
{
    public interface IKeyNormalizer
    {
        string Normalize(string key);
    }
}
