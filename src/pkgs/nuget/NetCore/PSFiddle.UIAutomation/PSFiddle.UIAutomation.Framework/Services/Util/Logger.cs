using MC.Track.TestSuite.Interfaces.Services;
using MC.Track.TestSuite.Interfaces.Util;
using MC.Track.TestSuite.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Services.Util
{
    public class Logger : ILogger
    {
        private event EventHandler<String> OnMessage;
        private static String _prefix = "";
        private Func<String, String> _tempFilter;
        private readonly IResolver resolver;
        private readonly IGenericScopingFactory genericScopingFactory;
        private readonly ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory;
        private bool useRawProvider = false;
        public Logger(IResolver resolver, IGenericScopingFactory genericScopingFactory, ISmartResourceDestroyerServiceFactory smartResourceDestroyerServiceFactory)
        {
            this._tempFilter = new Func<string, string>((message) =>
            {
                return $"{_prefix}> {message.Replace("\n", $"{_prefix} ")}\n";
            });
            this.resolver = resolver;
            this.genericScopingFactory = genericScopingFactory;
            this.smartResourceDestroyerServiceFactory = smartResourceDestroyerServiceFactory;
        }
        public ISmartResourceDestroyerService<EventHandler<String>> Subscribe(EventHandler<String> handler)
        {
            OnMessage += handler;
            return this.smartResourceDestroyerServiceFactory.Create<EventHandler<String>>(handler, (handlerToDistroy) =>
            {
                OnMessage -= handler;
            });
        }
        private void LogIt(String message, ConsoleColor color)
        {
            message = _tempFilter(message);
            OnMessage?.Invoke(Me(), message);
        }
        public void Dispose()
        {
            Me().IndentDown();
        }

        public void IndentDown()
        {
            _prefix=_prefix.Substring(0, Math.Max(_prefix.Length - 2, 0));
        }

        public void IndentUp()
        {
            _prefix += "  ";
        }

        public void LogError(string message)
        {
            LogIt(message, ConsoleColor.Red);
        }

        public void LogTrace(string message)
        {
            LogIt(message, ConsoleColor.Cyan);
        }

        public void LogWarning(string message)
        {
            LogIt(message, ConsoleColor.Yellow);
        }

        public ILogger Section(string Name)
        {
            Me().LogTrace($"{Name}");
            Me().IndentUp();
            return Me();
        }

        public void LogError(Exception e)
        {
            Me().LogError("Exception: ", e);
        }

        public void LogError(string message, Exception e)
        {
            Me().LogError($"{message} - {e.Message}\n{e.StackTrace}\n");
        }
        private ILogger Me()
        {
            return resolver.ApplyIntercepts<ILogger>(this);
        }
        public IScoper UseRawProvider()
        {
            var oldSetting = useRawProvider;
            useRawProvider = true;
            return genericScopingFactory.Create(() =>
            {
                useRawProvider = oldSetting;
            });
        }
    }
}
