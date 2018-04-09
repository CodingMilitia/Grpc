using System;
using System.Reflection;
using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;
using CodingMilitia.Grpc.Shared.Attributes;

namespace CodingMilitia.Grpc.Server.Internal
{
    internal static class GrpcHostFactory
    {
        public static GrpcHost<TService> Create<TService>(
            IServiceProvider appServices,
            GrpcServerOptions options,
            ISerializer serializer
        )
            where TService : class, IGrpcService
        {
            var builder = new GrpcHostBuilder<TService>(appServices);
            builder.SetOptions(options);
            builder.SetSerializer(serializer);
            builder.AddUnaryMethods();
            return builder.Build();
        }

        private static GrpcHostBuilder<TService> AddUnaryMethods<TService>(
            this GrpcHostBuilder<TService> builder
        )
            where TService : class, IGrpcService
        {
            //TODO: right now it goes through every method, these must be validated and filtered
            var serviceType = typeof(TService);
            var serviceName = ((GrpcServiceAttribute)serviceType.GetCustomAttribute(typeof(GrpcServiceAttribute))).Name ?? serviceType.Name;

            foreach (var method in serviceType.GetMethods())
            {
                var requestType = method.GetParameters()[0].ParameterType;
                var responseType = method.ReturnType.GenericTypeArguments[0];

                var handlerGenerator = typeof(MethodHandlerGenerator).GetMethod(nameof(MethodHandlerGenerator.GenerateUnaryMethodHandler));
                handlerGenerator = handlerGenerator.MakeGenericMethod(serviceType, requestType, responseType);
                var handler = handlerGenerator.Invoke(null, new[] { method });

                var addUnaryMethod = typeof(GrpcHostBuilder<TService>).GetMethod(nameof(GrpcHostBuilder<TService>.AddUnaryMethod), BindingFlags.Public | BindingFlags.Instance);
                addUnaryMethod = addUnaryMethod.MakeGenericMethod(requestType, responseType);

                var methodName = ((GrpcMethodAttribute)method.GetCustomAttribute(typeof(GrpcMethodAttribute))).Name ?? method.Name;

                addUnaryMethod.Invoke(builder, new[] { handler, serviceName, methodName });
            }

            return builder;
        }
    }
}