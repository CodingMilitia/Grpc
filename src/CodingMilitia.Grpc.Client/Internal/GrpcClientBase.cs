using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared.Internal;
using G = Grpc.Core;

namespace CodingMilitia.Grpc.Client.Internal
{
    public abstract class GrpcClientBase
    {
        private readonly G.Channel _channel;
        private readonly G.DefaultCallInvoker _invoker;
        private readonly ISerializer _serializer;

        protected GrpcClientBase(GrpcClientOptions options, ISerializer serializer)
        {
            _channel = new G.Channel(options.Url, options.Port, G.ChannelCredentials.Insecure);
            _invoker = new G.DefaultCallInvoker(_channel);
            _serializer = serializer;
        }

        protected async Task<TResponse> CallUnaryMethodAsync<TRequest, TResponse>(TRequest request, string serviceName, string methodName, CancellationToken ct)
            where TRequest : class
            where TResponse : class
        {
            var callOptions = new G.CallOptions(cancellationToken: ct);
            using (var call = _invoker.AsyncUnaryCall(GetMethodDefinition<TRequest, TResponse>(G.MethodType.Unary, serviceName, methodName), null, callOptions, request))
            {
                return await call.ResponseAsync.ConfigureAwait(false);
            }
        }

        private G.Method<TRequest, TResponse> GetMethodDefinition<TRequest, TResponse>(G.MethodType methodType, string serviceName, string methodName)
            where TRequest : class
            where TResponse : class
        {
            return MethodDefinitionGenerator.CreateMethodDefinition<TRequest, TResponse>(methodType, serviceName, methodName, _serializer);
        }
    }
}