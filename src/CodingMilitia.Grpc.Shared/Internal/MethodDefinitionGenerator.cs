using Grpc.Core;

namespace CodingMilitia.Grpc.Shared.Internal
{
    //TODO: review visibility
    public static class MethodDefinitionGenerator
    {
        public static Method<TRequest, TResponse> CreateMethodDefinition<TRequest, TResponse>(
            MethodType methodType,
            string serviceName,
            string methodName
        )
            where TRequest : class
            where TResponse : class
        {
            return new Method<TRequest, TResponse>(
                type: methodType,
                serviceName: serviceName,
                name: methodName,
                requestMarshaller: Marshallers.Create(
                    serializer: Serializer<TRequest>.ToBytes,
                    deserializer: Serializer<TRequest>.FromBytes
                ),
                responseMarshaller: Marshallers.Create(
                    serializer: Serializer<TResponse>.ToBytes,
                    deserializer: Serializer<TResponse>.FromBytes
                )
            );
        }
    }
}