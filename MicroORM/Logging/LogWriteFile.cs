using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace MicroORM.Logging
{
    public class LogWriteFile
    {

        bool IsEnabled(LogLevel logLevel)
        {
            return Convert.ToInt32(logLevel) >= Convert.ToInt32(FileLoggerOptions.MinLogLevel);
        }

        private static object l = new object();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public async Task WriteFileAsync(string text, LogLevel logLevel)
        {
            if (!IsEnabled(logLevel)) return;
            string logText = $"{DateTime.Now.ToString("u").PadRight(24)}  {logLevel.ToString().PadRight(12)}   {text}";
            string path = Path.Combine(FileLoggerOptions.FolderPath, DateTime.Now.ToString("yyyy-MM-dd-async")) + ".txt";
            await _semaphore.WaitAsync();
            try
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                    if (fi.Length > (1024 * 1024 * FileLoggerOptions.MaxFileCount))
                        path = Path.Combine(FileLoggerOptions.FolderPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm")) + ".txt";
                using (var fs = File.Open(path, FileMode.Append))
                {
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(logText);
                        sr.Close();
                    }
                    fs.Close();
                }
            }
            finally
            {
                _semaphore.Release();
            }

        }


        public void WriteFile(string text, LogLevel logLevel)
        {
            if (!IsEnabled(logLevel)) return;
            string logText = $"{logLevel.ToString().PadRight(12)} {DateTime.Now.ToString("u").PadRight(24)}   {text}";
            string path = Path.Combine(FileLoggerOptions.FolderPath, DateTime.Now.ToString("yyyy-MM-dd")) + ".txt";
            lock (l)
            {

                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                    if (fi.Length > (1024 * 1024 * FileLoggerOptions.MaxFileCount))
                        path = Path.Combine(FileLoggerOptions.FolderPath, DateTime.Now.ToString("yyyy-MM-dd HH-mm")) + ".txt";

                using (var fs = File.Open(path, FileMode.Append))
                {
                    using (var sr = new StreamWriter(fs))
                    {
                        sr.WriteLine(logText);
                        sr.Close();
                    }
                    fs.Close();
                }
            }

        }
    }


}
