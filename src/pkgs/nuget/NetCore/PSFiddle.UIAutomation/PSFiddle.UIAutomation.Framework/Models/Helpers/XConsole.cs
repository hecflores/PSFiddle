using MC.Track.TestSuite.Model.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Track.TestSuite.Model.Helpers
{
    public static class XConsole
    {
        private static Stack<Func<String, String>> _htmlProxyStack = null;
        private static Stack<Func<String, String>> HtmlProxyStack
        {
            get
            {
                if (_htmlProxyStack == null)
                {
                    _htmlProxyStack = new Stack<Func<string, string>>();
                    _htmlProxyStack.Push((value) => $"<b style='red'>Html Proxy Stack miss-aligned</b> | {value}");
                }

                return _htmlProxyStack;
            }
        }
        public static Func<String, String> HtmlTextProxy { get; set; } = (value) => value;

        private static Stack<Func<String, String>> _textProxyStack = null;
        private static Stack<Func<String, String>> TextProxyStack
        {
            get
            {
                if (_textProxyStack == null)
                {
                    _textProxyStack = new Stack<Func<string, string>>();
                    _textProxyStack.Push((value) => $"Text Proxy Stack miss-aligned | {value}");
                }

                return _textProxyStack;
            }
        }
        public static Func<String, String> TextProxy { get; set; } = (value) => value;
        private static event EventHandler<XConsoleEventArgs> Subscription;
        private static object _lock = new object();
        public static void Clean()
        {
            Subscription = null;
        }
        public static void ClearProxies()
        {
            HtmlProxyStack.Clear();
            TextProxyStack.Clear();

            TextProxy = (value) => value;
            HtmlTextProxy = (value) => value;

            HtmlProxyStack.Push(HtmlTextProxy);
            TextProxyStack.Push(TextProxy);
        }
        public static void PushHtmlProxy(Func<String, String> newProxy)
        {
            if (HtmlProxyStack.Count == 0)
                return;
            HtmlProxyStack.Push(HtmlTextProxy);
            HtmlTextProxy = newProxy;
        }
        public static void PopHtmlProxy()
        {
            if (HtmlProxyStack.Count == 0)
                return;
            HtmlTextProxy = HtmlProxyStack.Pop();
        }
        public static void PushTextProxy(Func<String, String> newProxy)
        {
            if (TextProxyStack.Count == 0)
                return;
            TextProxyStack.Push(TextProxy);
            TextProxy = newProxy;
        }
        public static void PopTextProxy()
        {
            if (TextProxyStack.Count == 0)
                return;
            TextProxy = TextProxyStack.Pop();
        }
        public static void Write(String value)
        {
            XConsoleEventArgs args = new XConsoleEventArgs();
            args.RawText = TextProxy?.Invoke(value) ?? value;
            args.HtmlText = HtmlTextProxy?.Invoke(value) ?? value;
            Subscription?.Invoke(null, args);
        }
        public static void WriteLine(String value)
        {
            XConsoleEventArgs args = new XConsoleEventArgs();
            args.RawText = TextProxy?.Invoke(value) ?? value;
            args.HtmlText = HtmlTextProxy?.Invoke(value) ?? value;
            Subscription?.Invoke(null, args);
            
        }

        public static void UnSubscribe(EventHandler<XConsoleEventArgs> handler)
        {
            Subscription -= handler;
        }
        public static EventHandler<XConsoleEventArgs> Listen(Action<String> callback)
        {
            var handler = new EventHandler<XConsoleEventArgs>((sender, arg) =>
            {
                try
                {
                    callback(arg.RawText);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"LOGGING FAILURE - {e.Message}");
                }

            });
            Subscription += handler;
            return handler;
        }
        public static EventHandler<XConsoleEventArgs> ListenConsole(Action<XConsoleEventArgs> callback)
        {
            var handler = new EventHandler<XConsoleEventArgs>((sender, arg) =>
            {
                try
                {
                    callback(arg);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"LOGGING FAILURE - {e.Message}");
                }

            });
            Subscription += handler;
            return handler;
        }
    }
}
