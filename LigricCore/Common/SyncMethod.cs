using System;
using System.Threading.Tasks;

namespace Common
{
    public class SyncMethod
    {
        private int oldNumber = -1;

        public async Task WaitingAnotherMethodsAsync(int number, Func<Task> action, int milliseconds = 10000)
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
                await action();

                oldNumber++;
                if (oldNumber != number)
                    throw new ArgumentException("Error message: \"Something wrong ;(((.");
            }
        }
    }
}
