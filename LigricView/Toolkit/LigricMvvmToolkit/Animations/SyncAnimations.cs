using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Animations
{
    public class SyncAnimations
    {
        protected int oldNumber = -1;

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

        public async Task ExecuteAnimationAsync(int number, Func<Storyboard> storyboardFunc, Action callBack = null, int millisecondsLimit = 10000)
        {
            bool isCompleted = false;
            Storyboard storyboard;
            int timeout = 0;
            int еxpectedNumber = number - 1;

            while (oldNumber < еxpectedNumber && timeout < millisecondsLimit)
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
                storyboard = storyboardFunc?.Invoke();
                try
                {
                    storyboard.Pause();
                }
                catch(Exception ex)
                {

                }

                EventHandler<object> completed = null;

                completed = (s, e) =>
                {
                    storyboard.Completed -= completed;

                    if (callBack != null)
                    {
                        callBack();
                    }

                    isCompleted = true;
                    oldNumber++;

                    if (oldNumber != number)
                    {
                        throw new ArgumentException($"Error message: \"An error occurred while synchronizing animations ;(((.\n Culprit of this event method: {nameof(ExecuteAnimation)}\"");
                    }
                };

                storyboard.Completed += completed;

                storyboard.Begin();

                while (!isCompleted)
                {
                    await Task.Delay(1);
                }
            }
        }
    }
}
