using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Shared.Internal;
using G = Grpc.Core;

namespace CodingMilitia.Grpc.Client.Internal
{
    public abstract class GrpcClientBase
    {
        private readonly G.Channel _channel;
        private readonly G.DefaultCallInvoker _invoker;

        //TODO: remove the hammer
        protected GrpcClientBase(GrpcClientBaseOptions options)
        {
            _channel = new G.Channel(options.Url, options.Port, G.ChannelCredentials.Insecure);
            _invoker = new G.DefaultCallInvoker(_channel);
        }

        protected async Task<TResponse> CallUnaryMethodAsync<TRequest, TResponse>(TRequest request, CancellationToken ct)
            where TRequest : class
            where TResponse : class
        {
            var callOptions = new G.CallOptions(cancellationToken: ct);
            using (var call = _invoker.AsyncUnaryCall(GetMethodDefinition<TRequest, TResponse>(G.MethodType.Unary), null, callOptions, request))
            {
                return await call.ResponseAsync.ConfigureAwait(false);
            }
        }

        private G.Method<TRequest, TResponse> GetMethodDefinition<TRequest, TResponse>(G.MethodType methodType)
            where TRequest : class
            where TResponse : class
        {
            return MethodDefinitionGenerator.CreateMethodDefinition<TRequest, TResponse>(methodType, "SampleService", "Send");
        }
    }
}