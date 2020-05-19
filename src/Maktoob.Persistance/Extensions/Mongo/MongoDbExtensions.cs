using JetBrains.Annotations;
using Maktoob.CrossCuttingConcerns.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Security.Authentication;

namespace Maktoob.Persistance.Extensions.Mongo
{
    public static class MongoDbExtensions
    {
        public static void AddMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>();
                var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
                mongoClient.Settings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                };
                return mongoClient;
            });
            services.AddScoped(serviceProvider =>
            {
                var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>();
                var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
                return mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            });
            RegisterConventions();
        }
        private static void RegisterConventions()
        {
            ConventionRegistry.Register("MaktoobConvention", new MongoConvention(), _ => true);
        }
    }
}
