using Grpc.Core;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Server
{
    public abstract class GrpcServiceBase : IGrpcService
    {
        public ServerCallContext Context { get; set; }
    }
}