using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utils
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


        private readonly HashSet<string> executeKeys = new HashSet<string>();

        public async void WaitingAnotherMethodsAsync(int number, Func<Task> action, string key, int millisecondsLimit = 10000)
        {
            bool canExecute = true;
            if (executeKeys.Contains(key))
            {
                canExecute = false;
            }
            else
            {
                executeKeys.Add(key);
            }
            int timeout = 0;
            int еxpectedNumber = number - 1;

            while (oldNumber < еxpectedNumber && timeout < millisecondsLimit)
            {
                timeout++;
                await Task.Delay(1);
            }

            if (!canExecute)
            {
                oldNumber++;
                return;
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
                executeKeys.Remove(key);
                oldNumber++;
                if (oldNumber != number)
                    throw new ArgumentException("Error message: \"Something wrong ;(((.");
            }
        }
    }
}
