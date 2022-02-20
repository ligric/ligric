using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Animations
{
    public class SyncAnimations
    {
        private int oldNumber = -1;

        public async void ExecuteAnimation(int number, Func<Storyboard> storyboardFunc, Action callBack = null, int milliseconds = 10000)
        {
            Storyboard stroyboard;
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
                stroyboard = storyboardFunc?.Invoke();
                stroyboard.Pause();

                EventHandler<object> completed = null;

                completed = (s, e) =>
                {
                    stroyboard.Completed -= completed;
                    oldNumber++;

                    if (callBack != null)
                    {
                        callBack();
                    }

                    if (oldNumber != number)
                    {
                        throw new ArgumentException($"Error message: \"An error occurred while synchronizing animations ;(((.\n Culprit of this event method: {nameof(ExecuteAnimation)}\"");
                    }
                };

                stroyboard.Completed += completed;

                stroyboard.Begin();
            }
        }
    }
}
