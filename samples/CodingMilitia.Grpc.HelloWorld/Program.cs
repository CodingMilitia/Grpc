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
                var client = new HammeredSampleServiceClient();
                var request = new SampleRequest { Value = 1 };
                var response = await client.SendAsync(request, CancellationToken.None);
                Console.WriteLine("{0} -> {1}", request.Value, response.Value);
            });

            await hostBuilder.RunConsoleAsync();
            await t;
        }

        class HammeredSampleServiceClient : ISampleService
        {
            private readonly G.Channel _channel;
            private readonly G.DefaultCallInvoker _invoker;
            private static readonly G.Method<SampleRequest, SampleResponse> Method 
                = MethodDefinitionGenerator.CreateMethodDefinition<SampleRequest, SampleResponse>(G.MethodType.Unary, "SampleService", "Send");

            public HammeredSampleServiceClient()
            {
                _channel = new G.Channel("127.0.0.1", 5000, G.ChannelCredentials.Insecure);
                _invoker = new G.DefaultCallInvoker(_channel);
            }

            public async Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct)
            {
                var callOptions = new G.CallOptions(cancellationToken: ct);
                using (var call = _invoker.AsyncUnaryCall(Method, null, callOptions, request))
                {
                    return await call.ResponseAsync.ConfigureAwait(false);
                }
            }

            public async Task ShutdownAsync()
            {
                await _channel.ShutdownAsync();
            }
        }
    }
}
