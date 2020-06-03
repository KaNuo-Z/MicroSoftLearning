using Learn.Data;
using Learn.Interface;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcServerWeb
{
    public class MyTimeService : ITimeService
    {
        public IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default)
            => SubscribeAsync(context.CancellationToken);

        private async IAsyncEnumerable<TimeResult> SubscribeAsync([EnumeratorCancellation] CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                yield return new TimeResult { Time = DateTime.UtcNow };
            }
        }
    }
}