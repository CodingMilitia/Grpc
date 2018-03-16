using System;
using CodingMilitia.Grpc.Server.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodingMilitia.Grpc.Server
{
    public static class GrpcHostExtensions
    {
        public static IServiceCollection AddGrpcServer<TService>(
            this IServiceCollection serviceCollection,
            Action<IGrpcHostBuilder<TService>> serviceConfigurator
        )
            where TService : class, IGrpcService
        {
            return AddGrpcServer<TService, TService>(serviceCollection, serviceConfigurator);
        }

        public static IServiceCollection AddGrpcServer<TServiceInterface, TServiceImplementation>(
            this IServiceCollection serviceCollection,
            Action<IGrpcHostBuilder<TServiceInterface>> serviceConfigurator
        )
            where TServiceInterface : class, IGrpcService
            where TServiceImplementation : class, IGrpcService, TServiceInterface
        {
            serviceCollection.AddScoped<TServiceInterface, TServiceImplementation>();
            serviceCollection.AddSingleton<GrpcHost<TServiceInterface>>(appServices =>
            {
                var builder = new GrpcHostBuilder<TServiceInterface>(appServices);
                serviceConfigurator(builder);
                return builder.Build();
            });
            serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService<TServiceInterface>>();
            return serviceCollection;
        }
    }
}