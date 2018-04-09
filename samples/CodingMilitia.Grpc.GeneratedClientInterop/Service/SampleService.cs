using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.Grpc.GeneratedClientInterop.Service
{
    class SampleService : ISampleService
    {
        public Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct)
        {
            return Task.FromResult(new SampleResponse
            {
                Value = request.Value + 1
            });
        }
    }
}