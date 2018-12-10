using log4net;
using log4net.Repository;
using System;
using System.Diagnostics;
using System.IO;

namespace Agile.Core
{
    /// <summary>
    /// log4net日志封装
    /// </summary>
    public class LogUtils
    {
        //NetStandard中log4net需要指定Repository
        public static ILoggerRepository loggerRepository = LogManager.CreateRepository("NetStandardRepository");

        //可以声明多个日志对象
        public static ILog log = LogManager.GetLogger(loggerRepository.Name, typeof(LogUtils));

        #region 01-初始化Log4net的配置
        /// <summary>
        /// 初始化Log4net的配置
        /// </summary>
        public static void InitLog4Net(string strLog4NetConfigFile)
        {
            //Path.Combine("Config", "log4net.xml")
            log4net.Config.XmlConfigurator.Configure(loggerRepository, new FileInfo(strLog4NetConfigFile));
        }
        #endregion




        /************************* 五种不同日志级别 *******************************/
        //FATAL(致命错误) > ERROR（一般错误） > WARN（警告） > INFO（一般信息） > DEBUG（调试信息）

        /// <summary>
        /// 将调试的信息输出，可以定位到具体的位置（解决高层封装带来的问题）
        /// </summary>
        /// <returns></returns>
        private static string GetDebugInfo(bool debugInfo)
        {
            if (!debugInfo)
                return "";

            StackTrace trace = new StackTrace(true);
            return trace.ToString();
        }

        #region 01-DEBUG（调试信息）
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Debug(string msg,bool debugInfo = false)
        {
            log.Debug(GetDebugInfo(debugInfo) + msg);
        }
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="exception">错误信息</param>
        public static void Debug(string msg, Exception exception, bool debugInfo = false)
        {
            log.Debug(GetDebugInfo(debugInfo) + msg, exception);
        }

        #endregion

        #region 02-INFO（一般信息）
        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Info(string msg, bool debugInfo = false)
        {
            log.Info(GetDebugInfo(debugInfo) + msg);
        }
        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="exception">错误信息</param>
        public static void Info(string msg, Exception exception, bool debugInfo = false)
        {
            log.Info(GetDebugInfo(debugInfo) + msg, exception);
        }
        #endregion

        #region 03-WARN（警告）
        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Warn(string msg, bool debugInfo = false)
        {
            log.Warn(GetDebugInfo(debugInfo) + msg);
        }
        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="exception">错误信息</param>
        public static void Warn(string msg, Exception exception, bool debugInfo = false)
        {
            log.Warn(GetDebugInfo(debugInfo) + msg, exception);
        }
        #endregion

        #region 04-ERROR（一般错误）
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Error(string msg, bool debugInfo = false)
        {
            log.Error(GetDebugInfo(debugInfo) + msg);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="exception">错误信息</param>
        public static void Error(string msg, Exception exception, bool debugInfo = false)
        {
            log.Error(GetDebugInfo(debugInfo) + msg, exception);
        }
        #endregion

        #region 05-FATAL(致命错误)
        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Fatal(string msg, bool debugInfo = false)
        {
            log.Fatal(GetDebugInfo(debugInfo) + msg);
        }


        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="exception">错误信息</param>
        public static void Fatal(string msg, Exception exception, bool debugInfo = false)
        {
            log.Fatal(GetDebugInfo(debugInfo) + msg, exception);
        }

        #endregion
    }
}
