using MC.Track.TestSuite.Interfaces.Attributes;
using MC.Track.TestSuite.Interfaces.Config;
using MC.Track.TestSuite.Interfaces.Dependencies;
using MC.Track.TestSuite.Interfaces.Driver;
using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Toolkit.Dependencies
{
    [AthenaRegister(typeof(IRunnerDependencies), Model.Enums.AthenaRegistrationType.Singleton)]
    public class RunnerDependencies : IRunnerDependencies
    {
        private readonly IBrowserDependencies browserDependencies;
        private readonly IConfiguration config;
        private readonly ITrackTestRunner trackTestRunner;
        private readonly ILogger logger;
        public RunnerDependencies(IResolver resolver)
        {
            this.browserDependencies = resolver.Resolve<IBrowserDependencies>();
            this.config = resolver.Resolve<IConfiguration>();
            this.trackTestRunner = resolver.Resolve<ITrackTestRunner>();
            this.logger = resolver.Resolve<ILogger>();
        }
        /// <summary>
        /// Loopings the action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action. The action that will be used to evaluate the passing of the loop</param>
        /// <param name="Title"></param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="Timeout"></param>
        /// <param name="DelayBetweenWaits"></param>
        public void TimeoutAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null)
        {
            Title = Title ?? "Looping Action";
            if (!Timeout.HasValue)
                Timeout = TimeSpan.FromMilliseconds(config.DefaultTimeoutTime);
            ExceptionOnEndingFailure = ExceptionOnEndingFailure ?? ((error) => error);

            Exception finalException = null;
            using (logger.Section(Title)) { 
            
                if (!trackTestRunner.WaitUntil(Title, () =>
                {
                    try
                    {
                        EvaluationAction();
                        OnSuccess?.Invoke();
                    }
                    catch (Exception e)
                    {
                        finalException = e;
                        SwallowAnyExceptions(() => OnFailureOfIteration?.Invoke(finalException));
                        return false;
                    }
                    return true;
                }, Timeout: Timeout))
                {
                    var oldException = finalException;
                    finalException = ExceptionOnEndingFailure.Invoke(finalException);
                    if (finalException == null)
                        finalException = new Exception($"After failuing with exception '{oldException.Message}' it was sent to be converted and was converted to null which is not supported", oldException);

                    SwallowAnyExceptions(() => OnFailure?.Invoke(finalException));
                    throw finalException;
                }
            }
            

        }

        /// <summary>
        /// Sections the specified name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="Exception">[Failed] - {Name}</exception>
        public void Section(String Name, Action callback)
        {
            using (logger.Section($"[Flow] - {Name}"))
            {
                try
                {
                    callback();
                    logger.LogTrace($"[Passed] - {Name}");
                }
                catch (Exception e)
                {
                    logger.LogError($"[Failed] - {Name}", e);
                    throw new Exception($"[Failed] - {Name}");
                }
            }
        }
        /// <summary>
        /// Sections the specified name.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <exception cref="Exception">[Failed] - {Name}</exception>
        public void TrySection(String Name, Action callback)
        {
            this.SwallowAnyExceptions(() =>
            {
                using (logger.Section($"{Name}"))
                {
                    callback();
                }
            });
            
        }
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
        public T TimeoutAction<T>(Func<T> EvaluationAction, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null)
        {
            DefaultOutput = DefaultOutput ?? (() => default(T));
            T item = DefaultOutput();
            this.TimeoutAction(() => { item = EvaluationAction(); }, Title, () => OnSuccess?.Invoke(item), OnFailureOfIteration, OnFailure, Timeout, DelayBetweenWaits, ExceptionOnEndingFailure);
            return item;
        }

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
        public bool TryTimeoutAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null)
        {
            return this.SwallowAnyExceptions(() => TimeoutAction(EvaluationAction, Title, OnSuccess, OnFailureOfIteration, OnFailure, Timeout, DelayBetweenWaits, ExceptionOnEndingFailure));
        }

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
        public bool TryTimeoutAction<T>(Func<T> EvaluationAction, out T Output, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, TimeSpan? Timeout = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null)
        {
            var _finalOutput = default(T);
            var _finalResult = this.SwallowAnyExceptions(() =>
            {
                _finalOutput = TimeoutAction(EvaluationAction, Title, OnSuccess, OnFailureOfIteration, OnFailure, Timeout, DelayBetweenWaits, ExceptionOnEndingFailure, DefaultOutput);
            });
            Output = _finalOutput;

            return _finalResult;
        }

        /// <summary>
        /// Swallows any exceptions.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public bool SwallowAnyExceptions(Action callback, Action<Exception> OnException = null)
        {
            OnException = OnException == null ? ((e) => logger.LogError("Swalling Error", e)) : OnException;
            try
            {
                callback();
                return true;
            }
            catch (Exception e)
            {
                OnException?.Invoke(e);
                return false;
            }
        }
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
        public bool TryRetryAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null)
        {
            return this.SwallowAnyExceptions(() => RetryAction(EvaluationAction, Title, OnSuccess, OnFailureOfIteration, OnFailure, NumberOfRetries, DelayBetweenWaits, ExceptionOnEndingFailure));
        }

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
        public bool TryRetryAction<T>(Func<T> EvaluationAction, out T Output, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null)
        {
            var _finalOutput = default(T);
            var _finalResult = this.SwallowAnyExceptions(() =>
            {
                _finalOutput = RetryAction(EvaluationAction, Title, OnSuccess, OnFailureOfIteration, OnFailure, NumberOfRetries, DelayBetweenWaits, ExceptionOnEndingFailure, DefaultOutput);
            });
            Output = _finalOutput;

            return _finalResult;
        }
        /// <summary>
        /// Retries the action.
        /// </summary>
        /// <param name="EvaluationAction">The evaluation action.</param>
        /// <param name="Title">The title.</param>
        /// <param name="OnSuccess">The on success.</param>
        /// <param name="OnFailureOfIteration">The on failure.</param>
        /// <param name="NumberOfRetries">The number of retries.</param>
        /// <param name="DelayBetweenWaits">The delay between waits.</param>
        public void RetryAction(Action EvaluationAction, String Title = null, Action OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null)
        {
            ExceptionOnEndingFailure = ExceptionOnEndingFailure==null?((e) => e): ExceptionOnEndingFailure;
            OnSuccess = OnSuccess == null ? (() => { }) : OnSuccess;
            Title = Title ?? "Looping Action";
            if (!NumberOfRetries.HasValue)
                NumberOfRetries = config.DefaultRetryAttempts;

            Exception finalException = null;
            using (logger.Section(Title))
            {
                if (!trackTestRunner.RetryBlock(Title, () =>
                {
                    try
                    {
                        EvaluationAction();
                        OnSuccess?.Invoke();
                    }
                    catch (Exception e)
                    {
                        finalException = e;
                        SwallowAnyExceptions(() => OnFailureOfIteration?.Invoke(e));
                        return false;
                    }
                    return true;

                }, RetryTimes: NumberOfRetries,
                   DelayBetweenWaits: DelayBetweenWaits))
                {
                    var oldException = finalException;
                    finalException = ExceptionOnEndingFailure.Invoke(finalException);
                    if (finalException == null)
                        finalException = new Exception($"After failuing with exception '{oldException.Message}' it was sent to be converted and was converted to null which is not supported", oldException);

                    SwallowAnyExceptions(() => OnFailure?.Invoke(finalException));
                    throw finalException;


                }
            }

        }

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
        public T RetryAction<T>(Func<T> EvaluationAction, String Title = null, Action<T> OnSuccess = null, Action<Exception> OnFailureOfIteration = null, Action<Exception> OnFailure = null, Int32? NumberOfRetries = null, TimeSpan? DelayBetweenWaits = null, Func<Exception, Exception> ExceptionOnEndingFailure = null, Func<T> DefaultOutput = null)
        {
            DefaultOutput = DefaultOutput == null ? (() => default(T)) : DefaultOutput;
            T item = DefaultOutput();
            this.RetryAction(() => { item = EvaluationAction(); }, Title, () => OnSuccess?.Invoke(item), OnFailureOfIteration, OnFailure, NumberOfRetries, DelayBetweenWaits, ExceptionOnEndingFailure);
            return item;
        }
    }
}
