using Learn.Data;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;

namespace Learn.Interface
{
    [ServiceContract]
    public interface ITimeService
    {
        IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default);
    }
}