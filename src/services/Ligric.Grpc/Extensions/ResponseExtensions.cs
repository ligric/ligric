using Ligric.Rpc.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace Ligric.Grpc
{
    public static class ResponseExtensions
    {
        public static ResponseResult GetSuccessResponseResult()
        {
            return new ResponseResult { IsSuccess = true, OccurredOn = Timestamp.FromDateTime(DateTime.UtcNow) };
        }

		public static ResponseResult GetFailedResponseResult()
		{
			return new ResponseResult { IsSuccess = false, OccurredOn = Timestamp.FromDateTime(DateTime.UtcNow) };
		}
	}
}
