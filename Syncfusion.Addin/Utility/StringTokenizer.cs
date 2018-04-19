using System;
using System.Collections;
using System.Collections.Generic;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// Class StringTokenizer.
    /// </summary>
    public class StringTokenizer : IEnumerable<string>
    {
        /// <summary>
        /// The default delimiters
        /// </summary>
        public const string DefaultDelimiters = " \n\t\r;+=-\"\')(}{][<>";

        /// <summary>
        /// The value
        /// </summary>
        private string value;

        /// <summary>
        /// The delimiters
        /// </summary>
        private string delimiters;

        /// <summary>
        /// The return delims
        /// </summary>
        private bool returnDelims;

        /// <summary>
        /// The split values
        /// </summary>
        private string[] splitValues;

        /// <summary>
        /// The is initialized
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// The this lock
        /// </summary>
        private object thisLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringTokenizer(string value)
            : this(value, DefaultDelimiters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delim">The delimiter.</param>
        public StringTokenizer(string value, string delim)
            : this(value, delim, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTokenizer"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="delim">The delimiter.</param>
        /// <param name="returnDelims">if set to <c>true</c> [return delims].</param>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public StringTokenizer(string value, string delim, bool returnDelims)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            if (string.IsNullOrEmpty(delim))
            {
                delim = DefaultDelimiters;
            }

            this.value = value;
            this.delimiters = delim;
            this.returnDelims = returnDelims;
            this.isInitialized = false;

            this.InitializeToken();
        }

        /// <summary>
        /// Initializes the token.
        /// </summary>
        /// <exception cref="System.NullReferenceException"></exception>
        private void InitializeToken()
        {
            if (!isInitialized)
            {
                lock (thisLock)
                {
                    if (!isInitialized)
                    {
                        if (string.IsNullOrEmpty(this.value))
                        {
                            throw new NullReferenceException();
                        }
                        char[] delimCharArray = this.delimiters.ToCharArray();
                        splitValues = this.value.Split(delimCharArray);
                    }
                }
            }
        }

        #region IEnumerable<string> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (string splitValue in splitValues)
            {
                yield return splitValue;
            }
        }

        #endregion IEnumerable<string> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable Members
    }
}