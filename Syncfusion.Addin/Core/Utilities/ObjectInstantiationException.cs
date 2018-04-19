using System;
using System.Runtime.Serialization;

namespace Syncfusion.Core
{
    /// <summary>
    /// Exception thrown whenever object instantiation fails.
    /// </summary>
    [Serializable]
    public class ObjectInstantiationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectInstantiationException()
            : base("Error instantiating object.")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public ObjectInstantiationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ObjectInstantiationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected ObjectInstantiationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}