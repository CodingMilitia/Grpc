using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Server
{
    public interface IGrpcHostBuilder<TService> where TService : class, IGrpcService
    {
        //TODO: server credentials and probably more stuff
        IGrpcHostBuilder<TService> SetUrl(string url);
        IGrpcHostBuilder<TService> SetPort(int port);
        IGrpcHostBuilder<TService> SetSerializer(ISerializer serializer);
    //     IGrpcHostBuilder<TService> AddUnaryMethod<TRequest, TResponse>(
    //        Func<TService, TRequest, CancellationToken, Task<TResponse>> handler,
    //        string serviceName,
    //        string methodName
    //    )
    //        where TRequest : class
    //        where TResponse : class;
    }
}