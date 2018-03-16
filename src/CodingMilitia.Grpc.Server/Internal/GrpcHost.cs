using System.Threading.Tasks;
using GrpcCore = Grpc.Core;

namespace CodingMilitia.Grpc.Server.Internal
{
    public class GrpcHost<TService> where TService : class, IGrpcService
    {
        GrpcCore.Server _server;

        public GrpcHost(GrpcCore.ServerServiceDefinition serviceDefinition)
        {
            _server = new GrpcCore.Server
            {
                Ports = { { "127.0.0.1", 5000, GrpcCore.ServerCredentials.Insecure } },
                Services =
                {
                    serviceDefinition
                }
            };
        }

        public void Start()
        {
            _server.Start();
        }

        public async Task StopAsync()
        {
            await _server.ShutdownAsync();
        }
    }
}