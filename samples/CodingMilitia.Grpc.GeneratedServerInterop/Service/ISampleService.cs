using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Shared;
using CodingMilitia.Grpc.Shared.Attributes;

namespace CodingMilitia.Grpc.GeneratedServerInterop.Service
{
    [GrpcService("SampleService")]
    public interface ISampleService : IGrpcService
    {
        [GrpcMethod("Send")]
        Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct);
    }
}