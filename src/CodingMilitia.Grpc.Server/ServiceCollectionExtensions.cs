using System;
using CodingMilitia.Grpc.Server.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CodingMilitia.Grpc.Shared;
using CodingMilitia.Grpc.Serializers;

namespace CodingMilitia.Grpc.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServer<TServiceInterface, TServiceImplementation>(
            this IServiceCollection serviceCollection,
            GrpcServerOptions options,
            ISerializer serializer
        )
            where TServiceInterface : class, IGrpcService
            where TServiceImplementation : class, IGrpcService, TServiceInterface
        {
            serviceCollection.AddScoped<TServiceInterface, TServiceImplementation>();
            serviceCollection.AddSingleton<GrpcHost<TServiceInterface>>(
                appServices => GrpcHostFactory.Create<TServiceInterface>(appServices, options, serializer)
            );

            serviceCollection.AddSingleton<IHostedService, GrpcBackgroundService<TServiceInterface>>();
            return serviceCollection;
        }
    }
}