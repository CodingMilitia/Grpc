using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace CodingMilitia.Grpc.Server
{
    public static class ServerExtensions
    {
        private static ISerializer Serializer = new ProtoBufSerializer();
        public static IServiceCollection AddGrpcServer<TServiceInterface, TServiceImplementation>(
            this IServiceCollection serviceCollection,
            GrpcServerOptions options
        )
            where TServiceInterface : class, IGrpcService
            where TServiceImplementation : class, IGrpcService, TServiceInterface
        {
            return serviceCollection.AddGrpcServer<TServiceInterface, TServiceImplementation>(options, Serializer);
        }
    }
}