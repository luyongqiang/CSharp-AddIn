using System;

namespace Syncfusion.Core.DesignPatterns
{
    /// <summary>
    /// Singleton Design Pattern, thread-safe without using locks.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    public sealed class Singleton<T> where T : new()
    {
        /// <summary>
        /// Class Inner.
        /// </summary>
        private class Inner
        {
            /// <summary>
            /// Static Field _instance.
            /// </summary>
            internal static T _instance;

            /// <summary>
            /// Initializes static members of the <see cref="T:Syncfusion.Core.DesignPatterns.Singleton`1.Inner" /> class.
            /// </summary>
            static Inner()
            {
                Singleton<T>.Inner._instance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
            }
        }

        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                return Singleton<T>.Inner._instance;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="T:Syncfusion.Core.DesignPatterns.Singleton`1" /> class from being created.
        /// </summary>
        private Singleton()
        {
        }
    }
}