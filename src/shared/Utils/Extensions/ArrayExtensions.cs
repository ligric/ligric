namespace Utils.Extensions
{
	public static class ArrayExtensions
	{
		public static void PushWithShift<T>(this T[] array)
		{
			if (array.Length == 0) return;

			for (int i = 0; i < array.Length; i++)
			{
				array[i + 1] = array[i];
				if (i == array.Length - 1) break;
			}
		}
	}
}
