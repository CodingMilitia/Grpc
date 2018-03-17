using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.HelloWorld.Service;
using CodingMilitia.Grpc.Server;
using CodingMilitia.Grpc.Shared.Attributes;
using CodingMilitia.Grpc.Shared.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using G = Grpc.Core;

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
                await Task.Delay(5000);
                await RunHammerClientAsync();
            });

            await hostBuilder.RunConsoleAsync();
            await t;
        }

        private static async Task RunHammerClientAsync()
        {
            var channel = new G.Channel("127.0.0.1", 5000, G.ChannelCredentials.Insecure);
            var invoker = new G.DefaultCallInvoker(channel);
            var method = MethodDefinitionGenerator
                .CreateMethodDefinition<SampleRequest,SampleResponse>(G.MethodType.Unary, "SampleService", "Send");
            
            using (var call = invoker.AsyncUnaryCall(method, null, new G.CallOptions { }, new SampleRequest { Value = 3 }))
            {
                var result = await call.ResponseAsync;
                Console.WriteLine(result.Value);
            }

            await channel.ShutdownAsync();
        }
    }

    

    
    

    


}
