using Maktoob.Domain.Repositories;
using Maktoob.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Maktoob.Persistance
{
    public static class ServicesExtensions
    {
        public static void AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

    }
}
