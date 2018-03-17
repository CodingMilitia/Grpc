using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Shared.Internal;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Server.Internal
{
    public class GrpcHostBuilder<TService> : IGrpcHostBuilder<TService> where TService : class, IGrpcService
    {
        private readonly IServiceProvider _appServices;
        private readonly ServerServiceDefinition.Builder _builder;
        private string _uri = "127.0.0.1";
        private int _port = 5000;

        public GrpcHostBuilder(IServiceProvider appServices)
        {
            _appServices = appServices;
            _builder = ServerServiceDefinition.CreateBuilder();
        }

        public IGrpcHostBuilder<TService> SetUri(string uri)
        {
            _uri = uri;
            return this;
        }

        public IGrpcHostBuilder<TService> SetPort(int port)
        {
            _port = port;
            return this;
        }

        public IGrpcHostBuilder<TService> AddUnaryMethod<TRequest, TResponse>(
            Func<TService, TRequest, CancellationToken, Task<TResponse>> handler,
            string serviceName,
            string methodName
        )
            where TRequest : class
            where TResponse : class
        {
            _builder.AddMethod(
                MethodDefinitionGenerator.CreateMethodDefinition<TRequest, TResponse>(MethodType.Unary, serviceName, methodName),
                async (request, context) =>
                {
                    using (var scope = _appServices.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<TService>();
                        var baseService = service as GrpcServiceBase;
                        if (baseService != null)
                        {
                            baseService.Context = context;
                        }
                        return await handler(service, request, CancellationToken.None).ConfigureAwait(false);
                    }
                }
            );
            return this;
        }

        

        internal GrpcHost<TService> Build()
        {
            var server = new global::Grpc.Core.Server
            {
                Ports = { { _uri, _port, ServerCredentials.Insecure } },
                Services =
                {
                    _builder.Build()
                }
            };
            return new GrpcHost<TService>(server);
        }
    }
}