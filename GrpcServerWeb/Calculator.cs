using Learn.Data;
using Learn.Interface;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace GrpcServerWeb
{
    public class Calculator : ICalculator
    {
        public ValueTask<MultiplyResult> MultiplyAsync(MultiplyRequest request, CallContext context = default)
        {
            return new ValueTask<MultiplyResult>(new MultiplyResult { Result = request.X * request.Y });
        }
    }
}