using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Executions
{
    public class CancelableExecution
    {
        private readonly bool _allowConcurrency;
        private Operation _activeOperation;

        // Represents a cancelable operation that signals its completion when disposed
        private class Operation : IDisposable
        {
            private readonly CancellationTokenSource _cts;
            private readonly TaskCompletionSource<bool> _completionSource;
            private bool _disposed;

            public Task Completion => _completionSource.Task; // Never fails

            public Operation(CancellationTokenSource cts)
            {
                _cts = cts;
                _completionSource = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public void Cancel() { lock (this) if (!_disposed) _cts.Cancel(); }

            void IDisposable.Dispose() // It is disposed once and only once
            {
                try { lock (this) { _cts.Dispose(); _disposed = true; } }
                finally { _completionSource.SetResult(true); }
            }
        }

        public CancelableExecution(bool allowConcurrency)
        {
            _allowConcurrency = allowConcurrency;
        }
        public CancelableExecution() : this(false) { }

        public bool IsRunning => Volatile.Read(ref _activeOperation) != null;

        public async Task<TResult> RunAsync<TResult>(
            Func<CancellationToken, Task<TResult>> action,
            CancellationToken extraToken = default)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var cts = CancellationTokenSource.CreateLinkedTokenSource(extraToken);
            using (var operation = new Operation(cts))
            {
                // Set this as the active operation
                var oldOperation = Interlocked.Exchange(ref _activeOperation, operation);
                try
                {
                    if (oldOperation != null && !_allowConcurrency)
                    {
                        oldOperation.Cancel();
                        await oldOperation.Completion; // Continue on captured context
                                                       // The Completion never fails
                    }
                    cts.Token.ThrowIfCancellationRequested();
                    var task = action(cts.Token); // Invoke on the initial context
                    return await task.ConfigureAwait(false);
                }
                finally
                {
                    // If this is still the active operation, set it back to null
                    Interlocked.CompareExchange(ref _activeOperation, null, operation);
                }
            }
            // The cts is disposed along with the operation
        }

        public Task RunAsync(Func<CancellationToken, Task> action,
            CancellationToken extraToken = default)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return RunAsync<object>(async ct =>
            {
                await action(ct).ConfigureAwait(false);
                return null;
            }, extraToken);
        }

        public Task CancelAsync()
        {
            var operation = Volatile.Read(ref _activeOperation);
            if (operation == null) return Task.CompletedTask;
            operation.Cancel();
            return operation.Completion;
        }

        public bool Cancel() => CancelAsync() != Task.CompletedTask;
    }
}
