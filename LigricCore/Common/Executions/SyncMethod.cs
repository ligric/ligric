using System;
using System.Threading.Tasks;

namespace Common
{
    public class SyncMethod
    {
        protected int oldNumber = -1;

        public async void WaitingAnotherMethodsAsync(int number, Action action, int millisecondsLimit = 10000)
        {
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
                action();

                oldNumber++;
                if (oldNumber != number)
                    throw new ArgumentException("Error message: \"Something wrong ;(((.");
            }
        }

        public async void WaitingAnotherMethodsAsync(int number, Func<Task> action, int millisecondsLimit = 10000)
        {
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
                await action();

                oldNumber++;
                if (oldNumber != number)
                    throw new ArgumentException("Error message: \"Something wrong ;(((.");
            }
        }
    }
}
