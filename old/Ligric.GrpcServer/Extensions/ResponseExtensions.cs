using Ligric.Protos;
using Google.Protobuf.WellKnownTypes;

namespace Ligric.GrpcServer
{
    public static class ResponseExtensions
    {
        public static ResponseResult GetSuccessResponseResult()
        {
            return new ResponseResult { IsSuccess = true, OccurredOn = Timestamp.FromDateTime(DateTime.UtcNow) };
        }
    }
}
