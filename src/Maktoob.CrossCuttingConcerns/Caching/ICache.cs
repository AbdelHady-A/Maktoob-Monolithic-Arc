using System;
using System.Threading.Tasks;

namespace Maktoob.CrossCuttingConcerns.Caching
{
    public interface ICache
    {
         Task Add(string key, object item, TimeSpan timeSpan);
         Task Get(string key);
         Task Remove(string key);
    }
}