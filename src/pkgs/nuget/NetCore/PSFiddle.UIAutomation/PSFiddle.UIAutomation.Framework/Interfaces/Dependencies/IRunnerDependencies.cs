using System;

namespace MC.Track.TestSuite.Interfaces.Dependencies
{
    public interface IRunnerDependencies
    {
        /// <summary>
        /// Sections the specified name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="callback">The callback.</param>
        void Section(String Name, Action callback);

        /// <summary>
        /// Tries the section.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="callback">The callback.</param>
        void TrySection(String Name, Action callback);

        /// <summary>
        /// Timeouts the action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        T TimeoutAction<T>(Func<T> EvaluationAction, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null);

        /// <summary>
        /// Timeouts the action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        void TimeoutAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null);

        /// <summary>
        /// Retries the action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="NumberOfRetries">The number of retries.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        void RetryAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null);

        /// <summary>
        /// Retries the action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="NumberOfRetries">The number of retries.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <returns></returns>
        T RetryAction<T>(Func<T> EvaluationAction, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null);

        /// <summary>
        /// Swallows any exceptions. If any exception occurs in the callback, it is consumed and sent to the "OnException" callback and returns false. Will not throw any exceptions
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="OnException">The on exception.</param>
        /// <returns></returns>
        bool SwallowAnyExceptions(Action callback, Action<Exception> OnException = null);

        /// <summary>
        /// Tries the timeout action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Output">The output.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <param name="ExceptionOnEndingFailure">The exception on ending failure.</param>
        /// <returns></returns>
        bool TryTimeoutAction<T>(Func<T> EvaluationAction, out T Output, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null);

        /// <summary>
        /// Tries the timeout action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="Timeout">The timeout.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <param name="ExceptionOnEndingFailure">The exception on ending failure.</param>
        /// <returns></returns>
        bool TryTimeoutAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null);

        /// <summary>
        /// Tries the retry action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="NumberOfRetries">The number of retries.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <param name="ExceptionOnEndingFailure">The exception on ending failure.</param>
        /// <returns></returns>
        bool TryRetryAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null);

        /// <summary>
        /// Tries the retry action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Output">The output.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="NumberOfRetries">The number of retries.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        /// <param name="ExceptionOnEndingFailure">The exception on ending failure.</param>
        /// <returns></returns>
        bool TryRetryAction<T>(Func<T> EvaluationAction, out T Output, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null);
    }
}