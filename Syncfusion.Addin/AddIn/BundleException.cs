using System;
using System.Runtime.Serialization;

namespace Syncfusion.Addin
{
    /// <summary>
    /// Class BundleException.
    /// </summary>
    [Serializable]
    public class BundleException : Exception
    {
        public BundleException()
        {
        }

        /// <summary>
        /// Bundle Exception
        /// </summary>
        /// <param name="message">Exception message</param>
        public BundleException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Bundle Exception
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">Exception</param>
        public BundleException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Bundle Exception
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected BundleException(SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}