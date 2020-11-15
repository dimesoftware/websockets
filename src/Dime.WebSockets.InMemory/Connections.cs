using System;
using System.Collections.Concurrent;

namespace Dime.WebSockets.InMemory
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Connections<T>
    {
        /// <summary>
        ///
        /// </summary>
        private Connections()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static Connections<T> Instance()
        {
            if (instance != null)
                return instance;

            lock (syncRoot)
            {
                instance ??= new Connections<T>();
            }

            return instance;
        }

        private static volatile Connections<T> instance;
        private static object syncRoot = new Object();
        public ConcurrentDictionary<string, T> Users = new ConcurrentDictionary<string, T>();
    }
}