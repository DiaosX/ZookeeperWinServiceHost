using System;
using System.IO;

namespace ZookeeperWinServiceHost
{
    /// <summary>
    /// 日志工具类
    /// </summary>
    public static class LogUtil
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public enum Type : sbyte
        {
            /// <summary>
            /// 警告级别
            /// </summary>
            Warnning = 1,
            /// <summary>
            /// 错误级别
            /// </summary>
            Error,
            /// <summary>
            /// 消息
            /// </summary>
            Info,
            /// <summary>
            /// 调试级别
            /// </summary>
            Debug,
            /// <summary>
            /// 致命级别
            /// </summary>
            Fatal
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        public static void Write(Exception ex, Type type)
        {
            if (ex == null)
            {
                return;
            }
            var message = ex.Message;
            GetInnerStackTrace(ex, ref message);
            Write(message, type);
        }
        /// <summary>
        /// 记录Info级别日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInfo(string message)
        {
            Write(message, Type.Info);
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteError(string message)
        {
            Write(message, Type.Error);
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWarnning(string message)
        {
            Write(message, Type.Warnning);
        }
        /// <summary>
        /// 记录致命日志
        /// </summary>
        /// <param name="message"></param>
        public static void WriteFatal(string message)
        {
            Write(message, Type.Fatal);
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteError(Exception ex)
        {
            Write(ex, Type.Error);
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteWarnning(Exception ex)
        {
            Write(ex, Type.Warnning);
        }
        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteInfo(Exception ex)
        {
            Write(ex, Type.Info);
        }
        /// <summary>
        /// 记录致命日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteFatal(Exception ex)
        {
            Write(ex, Type.Fatal);
        }

        private static void Write(string message, Type type)
        {
            try
            {
                switch (type)
                {
                    case Type.Warnning:
                        DiskLogInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Warnning:" + message);
                        break;
                    case Type.Error:
                        DiskLogInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Error:" + message);
                        break;
                    case Type.Info:
                        DiskLogInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Info:" + message);
                        break;
                    case Type.Debug:
                        DiskLogInfo(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " Debug:" + message);
                        break;
                }
            }
            catch
            {
                // ignored
            }
        }

        private static string GetInnerStackTrace(Exception ex, ref string stackTrace)
        {
            if (ex.InnerException != null)
            {
                return GetInnerStackTrace(ex.InnerException, ref stackTrace);
            }
            stackTrace = stackTrace + " \n " + ex.StackTrace;
            return stackTrace;
        }

        /// <summary>
        /// 磁盘Log,在系统目录的Log文件夹有规则的写出
        /// </summary>
        /// <param name="message">日志内容</param>
        private static void DiskLogInfo(string message)
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zookeeper-log");
                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }
                var logFile = Path.Combine(logPath, string.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd")));
                using (var writer = new StreamWriter(logFile, true))
                {
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

    }

}
