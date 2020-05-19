using System;
using System.Collections.Generic;
using System.Text;

namespace Maktoob.CrossCuttingConcerns.Options
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
