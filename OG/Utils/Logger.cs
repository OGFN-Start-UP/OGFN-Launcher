using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public static class Logger
    {
        private static TextWriter _writer;

        public static void Start()
        {
#if DEBUG
            Win32.AllocConsole();
#endif
            if (File.Exists(Constants.LogFile))
            {
                var directoryInfo = new DirectoryInfo(Constants.LogPath).GetFiles()
                    .OrderByDescending(x => x.LastWriteTimeUtc).ToList();

                var newestLog = directoryInfo.FirstOrDefault();
                var oldestLog = directoryInfo.LastOrDefault();

                if (directoryInfo.Count >= 20)
                    oldestLog.Delete();

                newestLog.MoveTo(newestLog.FullName.Replace(".log",
                    $"-backup-{newestLog.LastWriteTimeUtc:yyyy.MM.dd-HH.mm.ss}.log"));
            }

            _writer = File.CreateText(Constants.LogFile);
            _writer.WriteLine("Launcher log");
            _writer.WriteLine($"# Started on {DateTime.Now}");
            _writer.WriteLine();
            _writer.Flush();
        }

        public static void Log(string msg, LogLevel level = LogLevel.Info)
        {

            var method = new StackTrace().GetFrame(1).GetMethod();
            var typeName = method.ReflectedType.Name;
            var methodName = method.Name;

            if (methodName == ".ctor")
                methodName = "Constructor";

            if (typeName.Contains("Service"))
                typeName = typeName.Replace("Service", "");

            if (typeName.Contains('<'))
                typeName = typeName.Split('<')[1].Split('>')[0];

            Console.ForegroundColor = level switch
            {
                LogLevel.Debug => ConsoleColor.Cyan,
                LogLevel.Info => ConsoleColor.Green,
                LogLevel.Warning => ConsoleColor.DarkYellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Fatal => ConsoleColor.DarkRed,
                LogLevel.SwapInfo => ConsoleColor.Magenta
            };
#if RELEASE
            if (level == LogLevel.Debug)
                return;
#endif
            var text = $"[{DateTime.Now}] [Log{typeName}::{methodName} {level.GetDescription()}] {msg}";
            Console.WriteLine(text);

            _writer.WriteLine(text);
            _writer.Flush();
        }

        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                return null;

            var field = type.GetField(name);
            if (field == null)
                return null;

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                return attr.Description;

            return null;
        }
    }

    public enum LogLevel
    {
        [Description("DBG")] Debug,

        [Description("INF")] Info,

        [Description("WRN")] Warning,

        [Description("ERR")] Error,

        [Description("FTL")] Fatal,

        [Description("SWAP")] SwapInfo
    }
}
