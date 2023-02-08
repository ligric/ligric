using Uno.UI.Runtime.Skia;
using Windows.UI.Core;

namespace Ligric.UI.Uno
{
	internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.CursorVisible = false;

                var host = new FrameBufferHost(() =>
                {
					var window = CoreWindow.GetForCurrentThread();

					if (window != null)
					{
						window.KeyDown += (s, e) =>
						{
							if (e.VirtualKey == Windows.System.VirtualKey.F12)
							{
								Application.Current.Exit();
							}
						};
					}
				
                    return new App();
                });
                host.Run();
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}
