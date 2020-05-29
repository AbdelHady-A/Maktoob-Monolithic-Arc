using Maktoob.CrossCuttingConcerns.Error;
using Maktoob.CrossCuttingConcerns.Normalizers;
using Maktoob.Domain.Entities;
using Maktoob.Domain.Services;
using Maktoob.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maktoob.Domain
{
    public static class ServicesExtensions
    {
        public static void  AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IKeyNormalizer, NameNormalizer>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<GErrorDescriber>();
            services.AddValidators();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            List<Type> validatorTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => IsValidatorInterface(i)))
                .Where(t => t.Name.EndsWith("Validator"))
                .ToList();

            validatorTypes.ForEach(t =>
            {
                services.AddScoped(t.GetInterfaces().SingleOrDefault(i => IsValidatorInterface(i)), t);
            });
        }

        private static bool IsValidatorInterface(Type @interface)
        {
            if (!@interface.IsGenericType)
            {
                return false;
            }
            Type typeDefinition = @interface.GetGenericTypeDefinition();
            return typeDefinition == typeof(IValidator<>);
        }
    }
}
