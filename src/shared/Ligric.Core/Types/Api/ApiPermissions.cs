﻿using System;

namespace Ligric.Core.Ligric.Core.Types.Api
{
	[Flags]
	public enum ApiPermissions
	{
		None = 0,
		Read = 1,
		Activity = 2,
		Update = 4,
		Remove = 8,
		Share = 16,
	}
}
