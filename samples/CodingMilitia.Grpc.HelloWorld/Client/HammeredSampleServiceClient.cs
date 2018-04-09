using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Client;
using CodingMilitia.Grpc.Client.Internal;
using CodingMilitia.Grpc.HelloWorld.Service;
using CodingMilitia.Grpc.Serializers;

namespace CodingMilitia.Grpc.HelloWorld.Client
{
    class HammeredSampleServiceClient : GrpcClientBase, ISampleService
    {
        protected HammeredSampleServiceClient(GrpcClientOptions options) : base(options, new ProtoBufSerializer())
        {
        }

        public Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct)
        {
            return CallUnaryMethodAsync<SampleRequest, SampleResponse>(request, "SampleService", "Send", ct);
        }
    }
}