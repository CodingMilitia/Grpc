using System.Threading.Tasks;
using GrpcCore = Grpc.Core;
using CodingMilitia.Grpc.Shared;

namespace CodingMilitia.Grpc.Server.Internal
{
    public class GrpcHost<TService> where TService : class, IGrpcService
    {
        private readonly GrpcCore.Server _server;

        public GrpcHost(GrpcCore.Server server)
        {
            _server = server;
        }

        public void Start()
        {
            _server.Start();
        }

        public async Task StopAsync()
        {
            await _server.ShutdownAsync().ConfigureAwait(false);
        }
    }
}