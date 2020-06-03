using Grpc.Core;
using Grpc.Net.Client;
using Learn.Data;
using Learn.Interface;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using System;
using System.Threading;

namespace GrpcClient
{
    internal class Program
    {
        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            //使用 protobuf-net.grpc集成
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            using (var channel = GrpcChannel.ForAddress("http://localhost:50001"))
            {
                var calculator = channel.CreateGrpcService<ICalculator>();
                var result = await calculator.MultiplyAsync(new MultiplyRequest { X = 12, Y = 4 });
                Console.WriteLine(result.Result);

                var clock = channel.CreateGrpcService<ITimeService>();
                var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                var options = new CallOptions(cancellationToken: cancel.Token);
                await foreach (var time in clock.SubscribeAsync(new CallContext(options)))
                {
                    Console.WriteLine($"The time is now: {time.Time}");
                }

                Console.ReadLine();
            }
        }
    }
}