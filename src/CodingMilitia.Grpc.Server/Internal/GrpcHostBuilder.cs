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
    public class GrpcHostBuilder<TService> : IGrpcHostBuilder<TService> where TService : class, IGrpcService
    {
        private readonly IServiceProvider _appServices;
        private readonly ServerServiceDefinition.Builder _builder;
        private string _url = "127.0.0.1";
        private int _port = 5000;
        private ISerializer _serializer;

        public GrpcHostBuilder(IServiceProvider appServices)
        {
            _appServices = appServices;
            _builder = ServerServiceDefinition.CreateBuilder();
        }

        public IGrpcHostBuilder<TService> SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public IGrpcHostBuilder<TService> SetPort(int port)
        {
            _port = port;
            return this;
        }

        public IGrpcHostBuilder<TService> SetSerializer(ISerializer serializer)
        {
            _serializer = serializer;
            return this;
        }

        internal GrpcHost<TService> Build()
        {
            AddMethods();
            var server = new global::Grpc.Core.Server
            {
                Ports = { { _url, _port, ServerCredentials.Insecure } },
                Services =
                {
                    _builder.Build()
                }
            };
            return new GrpcHost<TService>(server);
        }

        private void AddMethods()
        {
            var serviceType = typeof(TService);
            var serviceName = ((GrpcServiceAttribute)serviceType.GetCustomAttribute(typeof(GrpcServiceAttribute))).Name ?? serviceType.Name;

            foreach (var method in serviceType.GetMethods())
            {
                var requestType = method.GetParameters()[0].ParameterType;
                var responseType = method.ReturnType.GenericTypeArguments[0];

                var handlerGenerator = typeof(MethodHandlerGenerator).GetMethod(nameof(MethodHandlerGenerator.GenerateUnaryMethodHandler));
                handlerGenerator = handlerGenerator.MakeGenericMethod(serviceType, requestType, responseType);
                var handler = handlerGenerator.Invoke(null, new[] { method });

                var addUnaryMethod = this.GetType().GetMethod(nameof(AddUnaryMethod), BindingFlags.NonPublic | BindingFlags.Instance);
                addUnaryMethod = addUnaryMethod.MakeGenericMethod(requestType, responseType);

                var methodName = ((GrpcMethodAttribute)method.GetCustomAttribute(typeof(GrpcMethodAttribute))).Name ?? method.Name;

                addUnaryMethod.Invoke(this, new[] { handler, serviceName, methodName });
            }
        }

        private IGrpcHostBuilder<TService> AddUnaryMethod<TRequest, TResponse>(
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
                        return await handler(service, request, CancellationToken.None).ConfigureAwait(false);
                    }
                }
            );
            return this;
        }
    }
}