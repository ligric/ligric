using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Utils.Extensions
{
	public static class EnumExtensions
	{
		public static T[] GetFlags<T>(this T flagsEnumValue) where T : Enum
		{
			return Enum
				.GetValues(typeof(T))
				.Cast<T>()
				.Where(e => flagsEnumValue.HasFlag(e))
				.ToArray();
		}

		public static bool HasFlag<T>(int currentFlugs, int neededFlags) where T : Enum
		{
			T current = (T)Enum.Parse(typeof(T), currentFlugs.ToString() ?? "0");
			T needed = (T)Enum.Parse(typeof(T), neededFlags.ToString() ?? "0");
			return current.HasFlag(needed);
		}
	}
}
