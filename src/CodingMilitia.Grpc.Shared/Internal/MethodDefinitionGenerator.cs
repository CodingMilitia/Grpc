using System;
using CodingMilitia.Grpc.Serializers;
using Grpc.Core;

namespace CodingMilitia.Grpc.Shared.Internal
{
    //TODO: review visibility
    public static class MethodDefinitionGenerator
    {
        public static Method<TRequest, TResponse> CreateMethodDefinition<TRequest, TResponse>(
            MethodType methodType,
            string serviceName,
            string methodName,
            ISerializer serializer
        )
            where TRequest : class
            where TResponse : class
        {
            return new Method<TRequest, TResponse>(
                type: methodType,
                serviceName: serviceName,
                name: methodName,
                requestMarshaller: Marshallers.Create(
                    serializer: serializer.ToBytes<TRequest>,
                    deserializer: serializer.FromBytes<TRequest>
                ),
                responseMarshaller: Marshallers.Create(
                    serializer: serializer.ToBytes<TResponse>,
                    deserializer: serializer.FromBytes<TResponse>
                )
            );
        }
    }
}