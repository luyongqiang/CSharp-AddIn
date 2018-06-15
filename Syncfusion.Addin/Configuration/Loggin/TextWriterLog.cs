using System;
using System.Diagnostics;
using System.IO;

namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// 文件写入
    /// </summary>
    internal class TextWriterLog : Log, IDisposable
    {
        private readonly TextWriterResponsibility _responsibility;
        private volatile bool _disposed;
        private TextWriter _writer;

        /// <summary>
        /// Gets or sets the writer.
        /// </summary>
        /// <value>The writer.</value>
        protected virtual TextWriter Writer
        {
            get
            {
                return _writer;
            }
            set
            {
                _writer = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLog"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public TextWriterLog(TextWriter writer)
            : this(writer, TextWriterResponsibility.DoesNotOwn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLog"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="responsibility">The responsibility.</param>
        /// <exception cref="System.ArgumentNullException">writer</exception>
        public TextWriterLog(TextWriter writer, TextWriterResponsibility responsibility)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            _responsibility = responsibility;
            _disposed = false;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Writer = writer;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The level.</param>
        /// <exception cref="System.ObjectDisposedException"></exception>
        public override void WriteLine(string message, LogLevel level)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            try
            {
                Writer.WriteLine(message);
                Writer.Flush();
            }
            catch (Exception ex)
            {
                Trace.TraceWarning(ex.Message, new object[]
                {
                    ex
                });
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            try
            {
                if (disposing && Writer != null)
                {
                    if (_responsibility == TextWriterResponsibility.Owns)
                    {
                        Writer.Dispose();
                    }
                    Writer = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Enum TextWriterResponsibility
    /// </summary>
    internal enum TextWriterResponsibility
    {
        /// <summary>
        /// The owns
        /// </summary>
        Owns,

        /// <summary>
        /// The does not own
        /// </summary>
        DoesNotOwn,

        /// <summary>
        /// My twos
        /// </summary>
        MyTwos,

        /// <summary>
        /// Any things
        /// </summary>
        AnyThings
    }
}