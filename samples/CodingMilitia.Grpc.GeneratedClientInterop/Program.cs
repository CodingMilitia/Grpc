using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.GeneratedClientInterop.Service;
using CodingMilitia.Grpc.Serializers;
using CodingMilitia.Grpc.Server;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodingMilitia.Grpc.GeneratedClientInterop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddGrpcServer<ISampleService, SampleService>(new GrpcServerOptions { Url = "127.0.0.1", Port = 5000 });
                });

            var cts = new CancellationTokenSource();

            var t = Task.Run(async () =>
            {
                Channel channel = null;
                try
                {
                    await Task.Delay(1000);
                    channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
                    var client = new Generated.SampleService.SampleServiceClient(channel);
                    var request = new Generated.SampleRequest { Value = 1 };
                    var response = await client.SendAsync(request);
                    Console.WriteLine("{0} -> {1}", request.Value, response.Value);
                    cts.Cancel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (channel != null)
                    {
                        await channel.ShutdownAsync();
                    }
                }
            });

            await serverHostBuilder.RunConsoleAsync(cts.Token);
            await t;
        }
    }
}
