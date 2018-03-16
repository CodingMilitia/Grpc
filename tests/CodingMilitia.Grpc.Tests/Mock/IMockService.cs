using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.Grpc.Tests.Mock
{
    public interface IMockService
    {
         Task<MockResponse> RequestAsync(MockRequest request, CancellationToken ct = default(CancellationToken));
    }
}