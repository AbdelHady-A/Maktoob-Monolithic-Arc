using JetBrains.Annotations;
using Maktoob.CrossCuttingConcerns.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Security.Authentication;

namespace Maktoob.Persistance.Extensions.Mongo
{
    public static class MongoDbExtensions
    {
        public static void AddMongoDb(this IServiceCollection services)
        {

            var mongoDbOptions = services.BuildServiceProvider().GetRequiredService<IOptions<MongoDbOptions>>();
            if (mongoDbOptions == null)
            {
                throw new ArgumentNullException(nameof(MongoDbOptions));
            }
            var options = mongoDbOptions.Value;

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var mongoClient = new MongoClient(options.ConnectionString);
                if (options.EnableSsl)
                {
                    mongoClient.Settings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = SslProtocols.Tls12
                    };
                }
                return mongoClient;
            });
            services.AddScoped(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
                return mongoClient.GetDatabase(options.DatabaseName);
            });
            RegisterConventions();
        }
        private static void RegisterConventions()
        {
            ConventionRegistry.Register("MaktoobConvention", new MongoDbConvention(), _ => true);
        }
    }
}
