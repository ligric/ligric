using Google.Protobuf.WellKnownTypes;
using Ligric.Protobuf;

namespace Ligric.Service.FuturesService.Api.Helpers
{
	public static class ResponseHelper
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
