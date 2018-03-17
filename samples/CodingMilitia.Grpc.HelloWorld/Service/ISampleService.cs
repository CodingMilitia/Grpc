using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Server;
using CodingMilitia.Grpc.Shared;
using CodingMilitia.Grpc.Shared.Attributes;

namespace CodingMilitia.Grpc.HelloWorld.Service
{
    [GrpcService("SampleService")] //TODO: not being used right now
    interface ISampleService : IGrpcService
    {
        Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct);
    }
}