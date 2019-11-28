using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace HeadphoneMonitorApp
{
    public static class ErrorLogger
    {
        private static string _fileNeme = Directory.GetCurrentDirectory() + "\\errorLog.txt";
        public static string FileNeme { get { return _fileNeme; }  set { _fileNeme = value; } }

        public enum ErrorType
        {
            [Description("Unhandled Exception")]
            UnhandledException,

            [Description("Custom")]
            Custom,

            [Description("Unknown")]
            Unknown
        }

        public static void Log(ErrorType errorType, string errMsg, DateTime time)
        {
            string errorLogMsg =
                "\r\n=============================================================\r\n" +
                "Error time: " + time.ToString() + "\r\n" +
                "Error type: " + errorType.ToDescription()  +
                "\r\n-------------------------------------------------------------\r\n" +
                "\r\n" +
                errMsg +
                "\r\n=============================================================\r\n";

            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open(FileNeme, FileMode.Append)))
                {
                    sw.Write(errorLogMsg);
                }
            }
            catch { }
        }
    }
}
