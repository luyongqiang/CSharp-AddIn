using Syncfusion.Addin.Utility;
using System;
using System.IO;
using System.Threading;

namespace Syncfusion.Addin.Configuration.Loggin
{
    /// <summary>
    /// Class FileLog.
    /// </summary>
    internal class FileLog : TextWriterLog
    {
        private readonly FileInfo _fileInfo;
        private FileStream _stream;
        private readonly object _syncRoot = new object();

        protected override TextWriter Writer
        {
            get
            {
                //判断文件长度
                if (_stream != null && _stream.Length > FileLogUtility.MaxFileSizeByte)
                {
                    object syncRoot;
                    Monitor.Enter(syncRoot = _syncRoot);
                    try
                    {
                        if (_stream != null && _stream.Length > FileLogUtility.MaxFileSizeByte)
                        {
                            _stream = null;
                            base.Writer.Dispose();
                            base.Writer = null;
                            if (FileLogUtility.CreateNewOnFileMaxSize)
                            {
                                File.Delete(_fileInfo.FullName);
                            }
                            else
                            {
                                var directoryName = _fileInfo.DirectoryName;
                                if (directoryName != null)
                                    File.Copy(_fileInfo.FullName, Path.Combine(directoryName, string.Format("log{0}.txt", DateTime.Now.ToString("yyyy-MM-dd-HH-mm"))));
                            }
                            base.Writer = CreateWriter(_fileInfo, false);
                            var streamWriter = base.Writer as StreamWriter;
                            if (streamWriter != null)
                                _stream = (streamWriter.BaseStream as FileStream);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(syncRoot);
                    }
                }
                return base.Writer;
            }
            set
            {
                base.Writer = value;
                var writer = base.Writer as StreamWriter;
                if (writer != null)
                    _stream = (writer.BaseStream as FileStream);
            }
        }

        private static TextWriter CreateWriter(FileInfo info, bool p)
        {
            FileStream stream = p ? info.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite) : info.Open(info.Exists ? FileMode.Truncate : FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            return new StreamWriter(stream);
        }

        public FileLog(string filePath) : this(filePath, true)
        {
        }

        public FileLog(string filePath, bool append) : this(new FileInfo(filePath), append)
        {
        }

        public FileLog(FileInfo info) : this(info, true)
        {
        }

        public FileLog(FileInfo info, bool append) : base(CreateWriter(info, append), TextWriterResponsibility.Owns)
        {
            _fileInfo = info;
        }
    }
}