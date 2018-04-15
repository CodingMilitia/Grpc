using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.Grpc.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcClient<TServiceInterface>(
            this IServiceCollection serviceCollection,
            GrpcClientOptions options,
            ISerializer serializer
        )
            where TServiceInterface : class, IGrpcService
        {
            serviceCollection.AddSingleton<TServiceInterface>(_ => GrpcClientFactory.Create<TServiceInterface>(options, serializer));
            return serviceCollection;
        }
    }
}