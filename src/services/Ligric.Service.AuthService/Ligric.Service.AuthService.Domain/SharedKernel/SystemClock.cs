using System;

namespace Ligric.Service.AuthService.Domain.SharedKernel
{
    public static class SystemClock
    {
        private static DateTime? _customDate;

        public static DateTime Now
        {
            get
            {
                if (_customDate.HasValue)
                {
                    return _customDate.Value;
                }

                return DateTime.UtcNow;
            }
        }

        public static void Set(DateTime customDate) => _customDate = customDate;

        public static void Reset() => _customDate = null;

		public static DateTime SetKind(this DateTime DT, DateTimeKind DTKind)
		{
			return DateTime.SpecifyKind(DT, DTKind);
		}
	}
}
