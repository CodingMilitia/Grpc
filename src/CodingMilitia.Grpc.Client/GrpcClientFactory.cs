using System;
using System.Linq;
using System.Reflection;
using CodingMilitia.Grpc.Client.Internal;
using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Client
{
    public static class GrpcClientFactory
    {
        public static TService Create<TService>(GrpcClientOptions options, ISerializer serializer) where TService : class, IGrpcService
        {
            var newType = new GrpcClientTypeBuilder().Create<TService>();
            return (TService)Activator.CreateInstance(newType, options, serializer);
        }
    }
}