using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel
{
    public static class ErrorHandlerExtensions
    {
        /// <summary>
        /// prototype #1, don't try this at home... it's too complex to make 15 or whatsoever overloads efficiently
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="func"></param>
        /// <param name="result"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        public static bool Try<TResult>(this ErrorHandler handler, object sender, Func<TResult> func, out TResult result, bool notify = true)
        {
            object tmp = null;            
            bool success = handler.Try(sender, (Delegate)func, out tmp, notify);
            
            result = success ? (TResult)tmp : default(TResult);
            
            return success;
        }


        /// <summary>
        /// Prototype #2 - simple chaining of delegates and notifying on demand
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="handler"></param>
        /// <param name="context"></param>
        /// <param name="func"></param>
        /// <param name="notifyOnError"></param>
        /// <returns></returns>
        public static Func<TResult> Try1<TResult>(this ErrorHandler handler, object context, Func<TResult> func, bool notifyOnError = true)
        {
            Func<TResult> r = delegate () 
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    if (notifyOnError)
                        handler.Notify(context, ex);
                }
                return default(TResult); 
            };

            return r;
        }

        public static Func<T> Try2<T>(this Func<T> func, ErrorHandler handler, object context, bool notify = true)
        {
            Func<T> r = delegate ()
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    if (notify)
                        handler.Notify(context, ex);
                }
                return default(T);
            };
            return r;
        }

        public static Func<T> LogOnError<T>(this Func<T> func, ErrorHandler handler, object context, bool catchException = true, bool notifyOnError = true)
        {
            Func<T> r = delegate()
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    if (notifyOnError)
                        handler.Notify(context, ex);
                    
                    if (catchException)
                        throw ex;
                }
                return default(T);
            };
            return r;
        }

        static object Fail()
        {
            // just throw sth. lol
            throw new Exception();            
        }

        static void TestTry()
        {
            Func<object> f = Fail;

            new ErrorHandler().Try1(null, f);

            f.LogOnError(new ErrorHandler(), null)();
        }
    }

}
