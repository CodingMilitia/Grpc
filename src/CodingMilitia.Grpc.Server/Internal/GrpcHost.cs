using CodingMilitia.Grpc.Shared;
using System.Threading.Tasks;
using GrpcCore = Grpc.Core;

namespace CodingMilitia.Grpc.Server.Internal
{
    internal class GrpcHost<TService> where TService : class, IGrpcService
    {
        private readonly GrpcCore.Server _server;

        public GrpcHost(GrpcCore.Server server)
        {
            _server = server;
        }

        public Task StartAsync()
        {
            _server.Start();
            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            await _server.ShutdownAsync().ConfigureAwait(false);
        }
    }
}