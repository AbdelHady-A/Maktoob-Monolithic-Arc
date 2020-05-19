using Maktoob.Application.Commands;
using Maktoob.Application.Decorators;
using Maktoob.Application.Queries;
using Maktoob.CrossCuttingConcerns.Options;
using Maktoob.Domain.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Maktoob.Application
{
    public static class ServicesExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMessageHandlers();
            services.AddEventHandlers();
            services.AddJwtBearerAuthentication();

        }
        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            services.AddScoped<IDispatcher, Dispatcher>();
            List<Type> handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => IsHandlerInterface(i)))
                .Where(t => t.Name.EndsWith("Handler"))
                .ToList();

            handlerTypes.ForEach(ht => { AddHandler(services, ht); });

            return services;
        }
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            List<Type> handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => IsEventHandlerInterface(i)))
                .ToList();

            handlerTypes.ForEach(ht => AddEventHandler(services, ht));
            return services;
        }
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services)
        {
            var jsonWebTokenOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JsonWebTokenOptions>>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenOptions.Value.Key));

            void JwtBearer(JwtBearerOptions options)
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = securityKey,
                    ValidIssuer = jsonWebTokenOptions.Value.Issuer,
                    ValidAudience = jsonWebTokenOptions.Value.Audience,
                    ValidateAudience = !string.IsNullOrWhiteSpace(jsonWebTokenOptions.Value.Audience),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jsonWebTokenOptions.Value.Issuer),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };
            }

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearer);
            return services;
        }
        private static void AddEventHandler(IServiceCollection services, Type type)
            => services.AddTransient(type);

        private static bool IsEventHandlerInterface(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>);

        private static void AddHandler(IServiceCollection services, Type type)
        {
            object[] attributes = type.GetCustomAttributes(false);

            List<Type> pipline = attributes
                .Select(a => ToDecorator(a))
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            Type interfaceType = type.GetInterfaces().Single(i => IsHandlerInterface(i));
            Func<IServiceProvider, object> factory = BuildPipline(pipline, interfaceType);
            services.AddScoped(interfaceType,factory);
        }

        private static Func<IServiceProvider, object> BuildPipline(List<Type> pipline, Type interfaceType)
        {
            List<ConstructorInfo> cotrs = pipline
                .Select(x =>
                {
                    Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                    return type.GetConstructors().Single();
                })
                .ToList();

            Func<IServiceProvider, object> func = provider =>
            {
                object current = null;
                cotrs.ForEach(cotr =>
                {
                    List<ParameterInfo> parameterInfos = cotr.GetParameters().ToList();
                    object[] parameters = GetParameters(parameterInfos, current, provider);

                    current = cotr.Invoke(parameters);
                });

                return current;
            };

            return func;
        }

        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];
            
            for(int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }
            return result;
        }

        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            Type parameterType = parameterInfo.ParameterType;
            if (IsHandlerInterface(parameterType))
            {
                return current;
            }

            object service = provider.GetService(parameterType);
            if(service != null)
            {
                return service;
            }
            throw new ArgumentException($"Type {parameterType} not found");
        }

        private static Type ToDecorator(object attribute)
        {
            Type type = attribute.GetType();

            if (type == typeof(DatabaseRetryAttribute))
            {
                return typeof(DatabaseRetryDecorator<,>);
            }

            if (type == typeof(AuditLoggingAttribute))
            {
                return typeof(AuditLoggingDecorator<,>);
            }

            throw new ArgumentException(attribute.ToString());

        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }
            Type typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<,>) || typeDefinition == typeof(IQueryHandler<,>);
        }
    }
}