using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SFLib
{
    public delegate void LoggerExitApplicationDelegate();

    /// <summary>
    /// 日志类。利用C#自带的Trace机制实现，使用本类的函数前，必须先注册适当的TraceListener。
    /// </summary>
    public static class Logger
    {
        public static LoggerExitApplicationDelegate ExitApplication = null;

        public static TextBoxTraceListener RegisterTextBoxListener(TextBoxBase output, int logMaxLine)
        {
            TextBoxTraceListener listener = new TextBoxTraceListener(output, logMaxLine);
            Trace.Listeners.Add(listener);
            return listener;
        }

        public static ConsoleTraceListener RegisterConsoleListener()
        {
            ConsoleTraceListener listener = new ConsoleTraceListener();
            Trace.Listeners.Add(listener);
            return listener;
        }

        public static LogFileTraceListener RegisterTextWriterListener()
        {
            LogFileTraceListener listener = new LogFileTraceListener();
            Trace.Listeners.Add(listener);
            return listener;
        }

        public static void Exception(string message)
        {
            WriteEntry(message, "异常");
        }

        public static void Exception(Exception ex)
        {
            WriteEntry(ex.Message, "异常");
        }

        public static void Throw(string message)
        {
            WriteEntry(message, "中断");
            throw new Exception(message);
        }

        public static void Throw(Exception ex)
        {
            WriteEntry(ex.Message, "中断");
            throw ex;
        }

        public static void QuietlyExit(string message)
        {
            WriteEntry(message, "退出");
            Environment.Exit(Environment.ExitCode);
        }

        public static void QuietlyExit(Exception ex)
        {
            WriteEntry(ex.Message, "退出");
            Environment.Exit(Environment.ExitCode);
        }

        public static void Exit(string message)
        {
            if (ExitApplication != null)
            {
                try
                {
                    ExitApplication();
                }
                catch
                { }
            }

            MessageBox.Show(message);
            WriteEntry(message, "退出");
            Environment.Exit(Environment.ExitCode);
        }

        public static void Exit(Exception ex)
        {
            if (ExitApplication != null)
            {
                try
                {
                    ExitApplication();
                }
                catch
                { }
            }

            MessageBox.Show(ex.Message);
            WriteEntry(ex.Message, "退出");
            Environment.Exit(Environment.ExitCode);
        }

        public static void Error(string message)
        {
            WriteEntry(message, "错误");
        }

        public static void Error(Exception ex)
        {
            WriteEntry(ex.Message, "错误");
        }

        public static void Warning(string message)
        {
            WriteEntry(message, "警告");
        }

        public static void Info(string message)
        {
            WriteEntry(message, "信息");
        }

        private static void WriteEntry(string message, string type)
        {
            Trace.WriteLine(
                    string.Format("[{0}][{1}] : {2}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  type,
                                  message));
        }
    }

    /// <summary>
    /// 将Trace结果写入一个TextBox的TraceListener。支持线程同步。
    /// </summary>
    public class TextBoxTraceListener : TraceListener
    {
        private TextBoxBase output;
        private int _logMaxLine;

        public TextBoxTraceListener(TextBoxBase output, int logMaxLine)
        {
            this.Name = "Trace";
            this.output = output;
            _logMaxLine = logMaxLine;
        }

        private delegate void AppendDelegate(string message);

        private void Append(string message)
        {
            CheckLogMaxLineForTextChangedEvent();
            if (!output.Disposing && !output.IsDisposed)
            {
                output.AppendText(message);
            }
            else
            {
                //todo: Server窗体被关闭后，log写入失败的处理
            }
        }

        private void CheckLogMaxLineForTextChangedEvent()
        {
            int retentionLineCount = 5;//刷新后保留的行数  若不保留，刚加入的Log就会被一起冲掉
            if (output.Lines.Length > _logMaxLine && output.Lines.Length >= retentionLineCount)
            {
                StringBuilder sbLog = new StringBuilder();
                for (int line = output.Lines.Length - (retentionLineCount + 1); line < output.Lines.Length; line++)
                {
                    AppendLine(sbLog, output.Lines[line]);
                }
                output.Text = sbLog.ToString();
            }
        }

        private void AppendLine(StringBuilder sb, string content)
        {
            if (sb.Length != 0)
            {
                sb.Append("\r\n");
            }
            sb.Append(content);
        }

        public override void Write(string message)
        {
            AppendDelegate append = new AppendDelegate(Append);
            if (output.InvokeRequired)
            {
                output.BeginInvoke(append, new object[] { message });
            }
            else
            {
                append(message);
            }
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }

    /// <summary>
    /// 将Trace结果写入一个文件的TraceListener。支持线程同步。
    /// </summary>
    public class LogFileTraceListener : TraceListener
    {
        private const int EARLY_DAY_LIMIT_COUNT = 30;
        private string _logFolder = AppDomain.CurrentDomain.BaseDirectory + "log\\";

        public LogFileTraceListener()
        {
            this.Name = "LogFileTrace";
        }

        public override void Write(string message)
        {
            CleanEarlyLog(_logFolder);
            File.AppendAllText(GetTodayLogFileName(), message);
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }

        private string GetTodayLogFileName()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".Log";
            return _logFolder + fileName;
        }

        private void CleanEarlyLog(string folderPath)
        {
            string logDateString = "";
            DateTime logDate = DateTime.Now;

            foreach (FileInfo logFileInfo in Directory.CreateDirectory(folderPath).GetFiles("*.Log"))
            {
                try
                {
                    logDateString = Path.GetFileNameWithoutExtension(logFileInfo.FullName);
                    logDate = DateTime.Parse(string.Format("{0}-{1}-{2}", logDateString.Substring(0, 4), logDateString.Substring(4, 2), logDateString.Substring(6)));
                    if (DateTime.Now.Subtract(logDate).TotalDays > EARLY_DAY_LIMIT_COUNT)
                    {
                        logFileInfo.Delete();
                    }
                }
                catch
                {

                }
            }
        }

    }

}