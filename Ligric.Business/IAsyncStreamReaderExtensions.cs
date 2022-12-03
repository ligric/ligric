using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Grpc.Core;
using System.Runtime.CompilerServices;

namespace Ligric.Business
{
    internal static class IAsyncStreamReaderExtensions
    {
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IAsyncStreamReader<T> asyncStreamReader)
        {
            if (asyncStreamReader is null) { throw new ArgumentNullException(nameof(asyncStreamReader)); }

            return new ToAsyncEnumerableEnumerable<T>(asyncStreamReader);
        }

        private sealed class ToAsyncEnumerableEnumerable<T> : IAsyncEnumerable<T>
        {
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => new ToAsyncEnumeratorEnumerator<T>(_asyncStreamReader, cancellationToken);

            private readonly IAsyncStreamReader<T> _asyncStreamReader;

            public ToAsyncEnumerableEnumerable(IAsyncStreamReader<T> asyncStreamReader)
            {
                _asyncStreamReader = asyncStreamReader;
            }

            private sealed class ToAsyncEnumeratorEnumerator<T2> : IAsyncEnumerator<T>
            {
                public T Current => _asyncStreamReader.Current;

                public async ValueTask<bool> MoveNextAsync() => await _asyncStreamReader.MoveNext(_cancellationToken);

                public ValueTask DisposeAsync() => default;

                private readonly IAsyncStreamReader<T> _asyncStreamReader;
                private readonly CancellationToken _cancellationToken;

                public ToAsyncEnumeratorEnumerator(IAsyncStreamReader<T> asyncStreamReader, CancellationToken cancellationToken)
                {
                    _asyncStreamReader = asyncStreamReader;
                    _cancellationToken = cancellationToken;
                }
            }
        }

        public static async IAsyncEnumerable<T> WithEnforcedCancellation<T>(this IAsyncEnumerable<T> source, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            cancellationToken.ThrowIfCancellationRequested();

            // Start the 'await foreach' without the new syntax
            // because we need access to the ValueTask returned by MoveNextAsync()
            var enumerator = source.GetAsyncEnumerator(cancellationToken);
            Task<bool> moveNext = null;

            // Combine MoveNextAsync() with another Task that can be awaited indefinitely,
            // until it throws OperationCanceledException
            var untilCanceled = UntilCanceled(cancellationToken);
            try
            {
                while (
                    await (
                        await Task.WhenAny(
                            (
                                moveNext = enumerator.MoveNextAsync().AsTask()
                            ),
                            untilCanceled
                        ).ConfigureAwait(false)
                    )
                )
                {
                    yield return enumerator.Current;
                }
            }
            finally
            {
                if (moveNext != null && !moveNext.IsCompleted)
                {
                    // Disable warning CS4014 "Because this call is not awaited, execution of the current method continues before the call is completed"
#pragma warning disable 4014 // This is the behavior we want!

                    moveNext.ContinueWith(async _ =>
                    {
                        await enumerator.DisposeAsync();
                    }, TaskScheduler.Default);
#pragma warning restore 4014
                }
                else if (enumerator != null)
                {
                    await enumerator.DisposeAsync();
                }
            }
        }

        private static Task<bool> UntilCanceled(CancellationToken cancellationToken)
        {
            return new Task<bool>(() => true, cancellationToken);
        }
    }
}
