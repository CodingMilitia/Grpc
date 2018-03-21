using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Client;
using CodingMilitia.Grpc.HelloWorld.Client;
using CodingMilitia.Grpc.HelloWorld.Service;
using CodingMilitia.Grpc.Server;
using Microsoft.Extensions.Hosting;

namespace CodingMilitia.Grpc.HelloWorld
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddGrpcServer<ISampleService, SampleService>(builder =>
                    {
                        builder.AddUnaryMethod(
                            (ISampleService service, SampleRequest request, CancellationToken ct) => service.SendAsync(request, ct),
                            "SampleService",
                            "Send"
                        );
                    });
                });

            var t = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(5000);
                    //var client = new HammeredSampleServiceClient();
                    var client = GrpcClientFactory.Generate<ISampleService>(new GrpcClientOptions { Url = "127.0.0.1", Port = 5000 });
                    var request = new SampleRequest { Value = 1 };
                    var response = await client.SendAsync(request, CancellationToken.None);
                    Console.WriteLine("{0} -> {1}", request.Value, response.Value);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            await hostBuilder.RunConsoleAsync();
            await t;
        }
    }
}