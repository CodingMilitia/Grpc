using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Shared.Internal;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using CodingMilitia.Grpc.Shared;
using System.Reflection;
using CodingMilitia.Grpc.Shared.Attributes;
using CodingMilitia.Grpc.Serializers;

namespace CodingMilitia.Grpc.Server.Internal
{
    internal class GrpcHostBuilder<TService> where TService : class, IGrpcService
    {
        private readonly IServiceProvider _appServices;
        private readonly ServerServiceDefinition.Builder _builder;
        private GrpcServerOptions _options;
        private ISerializer _serializer;

        public GrpcHostBuilder(IServiceProvider appServices)
        {
            _appServices = appServices;
            _builder = ServerServiceDefinition.CreateBuilder();
        }

        public GrpcHostBuilder<TService> SetOptions(GrpcServerOptions options)
        {
            _options = options;
            return this;
        }

        public GrpcHostBuilder<TService> SetSerializer(ISerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        public GrpcHost<TService> Build()
        {
            var server = new global::Grpc.Core.Server
            {
                Ports = { { _options.Url, _options.Port, ServerCredentials.Insecure } },
                Services =
                {
                    _builder.Build()
                }
            };
            return new GrpcHost<TService>(server);
        }

        public GrpcHostBuilder<TService> AddUnaryMethod<TRequest, TResponse>(
            Func<TService, TRequest, CancellationToken, Task<TResponse>> handler,
            string serviceName,
            string methodName
        )
            where TRequest : class
            where TResponse : class
        {
            _builder.AddMethod(
                MethodDefinitionGenerator.CreateMethodDefinition<TRequest, TResponse>(MethodType.Unary, serviceName, methodName, _serializer),
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
                        return await handler(service, request, context.CancellationToken).ConfigureAwait(false);
                    }
                }
            );
            return this;
        }
    }
}