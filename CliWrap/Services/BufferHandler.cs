using System;

namespace CliWrap.Services
{
    /// <summary>
    /// The default implementation of <see cref="IBufferHandler"/> which uses delegates
    /// </summary>
    public class BufferHandler : IBufferHandler
    {
        private readonly Action<string> _standardOutputHandler;
        private readonly Action<string> _standardErrorHandler;

        /// <summary />
        public BufferHandler(Action<string> standardOutputHandler = null, Action<string> standardErrorHandler = null)
        {
            _standardOutputHandler = standardOutputHandler;
            _standardErrorHandler = standardErrorHandler;
        }

        /// <inheritdoc />
        public void HandleStandardOutput(string line)
        {
            _standardOutputHandler?.Invoke(line);
        }

        /// <inheritdoc />
        public void HandleStandardError(string line)
        {
            _standardErrorHandler?.Invoke(line);
        }
    }

    /// <summary>
    /// Extensions methods to help with backward-compatibility.
    /// </summary>
    public static class BufferHandlerExtensions
    {
        /// <summary>
        /// Creates an <see cref="IObserver{T}"/> that delegates to the
        /// standard output handler of the given <see cref="IBufferHandler"/>.
        /// </summary>
        public static IObserver<string> CreateOutputObserver(this IBufferHandler handler) =>
            new Observer(handler.HandleStandardOutput);

        /// <summary>
        /// Creates an <see cref="IObserver{T}"/> that delegates to the
        /// standard error handler of the given <see cref="IBufferHandler"/>.
        /// </summary>
        public static IObserver<string> CreateErrorObserver(this IBufferHandler handler) =>
            new Observer(handler.HandleStandardError);

        sealed class Observer : IObserver<string>
        {
            readonly Action<string> _action;

            public Observer(Action<string> action) => _action = action;

            public void OnCompleted() {}
            public void OnError(Exception error) {}
            public void OnNext(string value) => _action(value);
        }
    }
}