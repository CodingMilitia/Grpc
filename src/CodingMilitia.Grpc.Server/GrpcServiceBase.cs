using Grpc.Core;

namespace CodingMilitia.Grpc.Server
{
    public abstract class GrpcServiceBase : IGrpcService
    {
        public ServerCallContext Context { get; set; }
    }
}