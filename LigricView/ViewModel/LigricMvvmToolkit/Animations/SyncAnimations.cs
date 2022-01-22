using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Animation;

namespace LigricMvvmToolkit.Animations
{
    public delegate void ExecuteAnimationEventArgs(object sender, int number);
    public class SyncAnimations
    {
        private int oldNumber = -1;

        private Storyboard stroyboard;
        public async void ExecuteAnimation(int number, Func<Storyboard> storyboardFunc, Action callBack = null, int milliseconds = 10000)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
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
                    stroyboard?.Pause();

                    stroyboard = storyboardFunc?.Invoke();

                    stroyboard.Completed += (s, e) =>
                    {
                        callBack();

                        oldNumber++;

                        if (oldNumber != number)
                            throw new ArgumentException("Error message: \"Something wrong ;(((.");
                    };
                    stroyboard.Begin();
                }
            });
        }
    }
}
