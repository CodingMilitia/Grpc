using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Client;
using CodingMilitia.Grpc.Serializers;
using G = Grpc.Core;

namespace CodingMilitia.Grpc.GeneratedServerInterop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new G.Server
            {
                Services = { Generated.SampleService.BindService(new SampleServiceImplementation()) },
                Ports = { new G.ServerPort("127.0.0.1", 5000, G.ServerCredentials.Insecure) }
            };
            server.Start();

            var t = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000);
                    var client = GrpcClientFactory.Create<Service.ISampleService>(new GrpcClientOptions { Url = "127.0.0.1", Port = 5000 }, new ProtoBufSerializer());
                    var request = new Service.SampleRequest { Value = 1 };
                    var response = await client.SendAsync(request, CancellationToken.None);
                    Console.WriteLine("{0} -> {1}", request.Value, response.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            await t;
            await server.ShutdownAsync();
        }
    }

    public class SampleServiceImplementation : Generated.SampleService.SampleServiceBase
    {
        public override Task<Generated.SampleResponse> Send(Generated.SampleRequest request, G.ServerCallContext context)
        {
            return Task.FromResult(new Generated.SampleResponse { Value = request.Value + 1 });
        }
    }
}
