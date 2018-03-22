using System;
using System.Linq;
using System.Reflection;
using CodingMilitia.Grpc.Client.Internal;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Client
{
    public static class GrpcClientFactory
    {
        public static TService Generate<TService>(GrpcClientOptions options) where TService : class, IGrpcService
        {
            var newType = new GrpcClientTypeBuilder().Create<TService>();
            return (TService)Activator.CreateInstance(newType, options);
        }
    }
}