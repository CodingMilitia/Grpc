using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.Grpc.Client
{
    public static class ClientExtensions
    {
        private static ISerializer Serializer = new ProtoBufSerializer();
        
        public static IServiceCollection AddGrpcClient<TServiceInterface>(
            this IServiceCollection serviceCollection,
            GrpcClientOptions options
        )
            where TServiceInterface : class, IGrpcService
        {
            serviceCollection.AddGrpcClient<TServiceInterface>(options, Serializer);
            return serviceCollection;
        }
    }
}