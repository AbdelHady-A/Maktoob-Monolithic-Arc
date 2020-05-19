using System.Collections.Generic;
using MongoDB.Bson.Serialization.Conventions;

namespace Maktoob.Persistance.Extensions.Mongo
{
    internal class MongoConvention : IConventionPack
    {
        public IEnumerable<IConvention> Conventions => new List<IConvention>
        {
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(MongoDB.Bson.BsonType.String),
            new CamelCaseElementNameConvention()
        };
    }
}