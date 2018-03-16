using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.Grpc.Server
{
    public interface IGrpcHostBuilder<TService> where TService : class, IGrpcService
    {
        //TODO: server credentials and probably more stuff
        IGrpcHostBuilder<TService> SetUri(Uri uri);
        IGrpcHostBuilder<TService> SetPort(int port);
        IGrpcHostBuilder<TService> AddUnaryMethod<TRequest, TResponse>(
           Func<TService, TRequest, CancellationToken, Task<TResponse>> handler,
           string serviceName,
           string methodName
       )
           where TRequest : class
           where TResponse : class;
    }
}