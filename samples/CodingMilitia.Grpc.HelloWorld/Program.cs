using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.Grpc.Server;
using CodingMilitia.Grpc.Shared.Attributes;
using Microsoft.Extensions.DependencyInjection;
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

            await hostBuilder.RunConsoleAsync();
        }
    }

    [GrpcService("SampleService")]
    interface ISampleService : IGrpcService
    {
        Task<SampleResponse> SendAsync(SampleRequest request, CancellationToken ct);
    }

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

    class SampleRequest
    {
        public int Value { get; set; }
    }

    class SampleResponse
    {
        public int Value { get; set; }
    }
}
