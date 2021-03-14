using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroORM.Logging
{
    public static class FileLoggerOptions
    {
        

        private static int maxFileMB;
        public static int MaxFileMB
        {
            get { return maxFileMB > 0 ? maxFileMB : 2; }
            set { maxFileMB = value; }
        }

        private static string folderPath;
        public static string FolderPath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(folderPath) ?
                  folderPath : Path.Combine(Directory.GetCurrentDirectory(), @"..\..\");
            }
            set { folderPath = value; }
        }

        private static int maxFileCount;

        public static int MaxFileCount
        {
            get { return maxFileCount >= 7 ? maxFileCount : 7; }
            set { maxFileCount = value; }
        }

        public static LogLevel MinLogLevel { get; set; } = LogLevel.Information;

    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }
}
