using MicroORM.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MicroORM
{
    public static class AllConfig
    {
        public static void SetConfig(Action<Config> config)
        {
            Config c= new Config();
            config.Invoke(c);
            

            if (c.IsDbLogger)
                DBLoggerOptions.IsDbLogger = c.IsDbLogger;

            if (!string.IsNullOrEmpty(c.LogDbName))
                DBLoggerOptions.LogDbName = c.LogDbName;

            if (c.MaxFileMB>0)
                FileLoggerOptions.MaxFileMB = c.MaxFileMB;

            if (!string.IsNullOrEmpty(c.FolderPath))
                FileLoggerOptions.FolderPath = c.FolderPath;

            if (c.IsFileLog)
                FileLoggerOptions.IsFileLog = c.IsFileLog;

            if (c.MaxFileCount > 0)
                FileLoggerOptions.MaxFileCount = c.MaxFileCount;

            if (c.MinLogLevel!=default)
                FileLoggerOptions.MinLogLevel = c.MinLogLevel;

            if (!string.IsNullOrEmpty(c.ConnectionString))
                ORMConfig.ConnectionString = c.ConnectionString;

            if (c.DbType != default)
                ORMConfig.DbType = c.DbType;

            if (c.Domain != default)
                ORMConfig.domain = c.Domain;
        }
    }


    public class Config
    {        
        public bool IsDbLogger { get; set; }
        /// <summary>
        /// default value AppLog
        /// </summary>
        public string LogDbName { get; set; } = "AppLog";

        private static int maxFileMB;
        public int MaxFileMB
        {
            get { return maxFileMB > 0 ? maxFileMB : 2; }
            set { maxFileMB = value; }
        }

        private  string folderPath;
        public  string FolderPath
        {
            get
            {
                return !string.IsNullOrWhiteSpace(folderPath) ?
                  folderPath : Path.Combine(Directory.GetCurrentDirectory(), @"..\..\");
            }
            set { folderPath = value; }
        }
        public  bool IsFileLog { get; set; } = true;

        private  int maxFileCount;

        public  int MaxFileCount
        {
            get { return maxFileCount >= 7 ? maxFileCount : 7; }
            set { maxFileCount = value; }
        }
        public  LogLevel MinLogLevel { get; set; } = LogLevel.Information;

        public string ConnectionString { get; set; } = "";
        public DbType DbType { get; set; }
       
        public AppDomain Domain { get; set; }
    }
}
