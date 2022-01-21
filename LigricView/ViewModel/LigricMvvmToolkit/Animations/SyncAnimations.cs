using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Animations
{
    public class SyncAnimations
    {
        private int oldNumber = -1;

        public async Task ExecuteAnimation(int number, Func<Storyboard> storyboardFunc, int milliseconds = 10000)
        {
            int timeout = 0;
            int еxpectedNumber = number - 1;

            while (oldNumber < еxpectedNumber && timeout < milliseconds)
            {
                timeout++;
                await Task.Delay(1);
            }

            if (oldNumber < еxpectedNumber)
            {
                throw new ArgumentException($"Error message: \"Message after the {oldNumber} was lost.\"");
            }
            else if (oldNumber > еxpectedNumber)
            {
                throw new ArgumentException($"Error message: \"Message after the {oldNumber} already finished.\"");
            }
            else
            {
                var stroyboard = storyboardFunc?.Invoke();

                stroyboard.Completed += (s, e) =>
                {
                    oldNumber++;

                    if (oldNumber != number)
                        throw new ArgumentException("Error message: \"Something wrong ;(((.");
                };
                stroyboard.Begin();
            }
        }
    }
}
