using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using WinCopies.Util;

using static System.Console;

using static WinCopies
#if !WinCopies3
    .Util
#endif
    .ThrowHelper;

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public struct ConsoleLogger
    {
        public byte TabsCount { get; private set; }

        public ConsoleLogger(in byte initialTabsCount) => TabsCount = initialTabsCount;

        public void WriteLine(string
#if CS8
            ?
#endif
            msg, bool? increment, ConsoleColor? color = null)
        {
            if (color.HasValue)

                ForegroundColor = color.Value;

            System.Console.WriteLine(new string('\t', increment.HasValue ? increment.Value ? ++TabsCount : TabsCount-- : TabsCount) + msg);

            ResetColor();
        }
    }

    public delegate void Logger(string
#if CS8
        ?
#endif
        server, bool? increment, ConsoleColor? consoleColor = null);

    namespace Extensions
    {
        public enum LoggingLevel
        {
            Normal = 0,
            Information,
            Success,
            Warning,
            Error
        }

        public interface ILogger
        {
            byte TabsCount { get; }

            void WriteLine(string
#if CS8
                ?
#endif
                msg, bool? increment, LoggingLevel level);
        }

        public abstract class LoggerBase : ILogger
        {
            public byte TabsCount { get; private set; }

            public LoggerBase(in byte initialTabsCount = 0) => TabsCount = (byte)(initialTabsCount + 1);

            protected abstract void WriteLineOverride(string
#if CS8
                ?
#endif
                msg, LoggingLevel level);

            public void WriteLine(string
#if CS8
                ?
#endif
                msg, bool? increment, LoggingLevel level) => WriteLineOverride($"{DateTime.Now}{new string('\t', increment.HasValue ? increment.Value ? ++TabsCount : TabsCount-- : TabsCount)}{msg}", level);
        }

        public class ConsoleLogger : LoggerBase
        {
            public ConsoleLogger(in byte initialTabsCount = 0) : base(initialTabsCount) { /* Left empty. */ }

            public static Logger GetLogger(in byte initialTabsCount = 0) => new ConsoleLogger(initialTabsCount).WriteLine;

            protected override void WriteLineOverride(string
#if CS8
                ?
#endif
                msg, LoggingLevel level)
            {
                void writeLine() => System.Console.WriteLine(msg);

                void onNormalLevel()
                {
                    ResetColor();

                    writeLine();
                }

                if (level == LoggingLevel.Normal)

                    onNormalLevel();

                else
                {
                    ConsoleColor? getColor()
#if CS8
                        =>
#else
                {
                    switch (
#endif
                        level
#if CS8
                            switch
#else
                    )
#endif
                        {
#if !CS8
                        case
#endif
                            LoggingLevel.Information
#if CS8
                                    =>
#else
                    :
                            return
#endif
                                    ConsoleColor.Blue
#if CS8
                                ,
#else
                            ;
                        case
#endif
                            LoggingLevel.Success
#if CS8
                            =>
#else
                    :
                            return
#endif
                            ConsoleColor.DarkGreen
#if CS8
                                ,
#else
                            ;
                        case
#endif
                            LoggingLevel.Warning
#if CS8
                                    =>
#else
                    :
                            return
#endif
                                    ConsoleColor.DarkYellow
#if CS8
                                ,
#else
                            ;
                        case
#endif
                            LoggingLevel.Error
#if CS8
                                    =>
#else
                    :
                            return
#endif
                                    ConsoleColor.DarkRed
#if CS8
                                ,
                            _ =>
#else
                            ;
                        default:
                            return
#endif
                    null
#if CS8
                        };
#else
                    ;
                    }
                }
#endif
                    ConsoleColor? consoleColor = getColor();

                    if (consoleColor.HasValue)
                    {
                        ForegroundColor = consoleColor.Value;

                        writeLine();

                        ResetColor();
                    }

                    else

                        onNormalLevel();
                }
            }
        }

        public class FileLogger : LoggerBase
        {
            public StreamWriter Writer { get; }

            public FileLogger(in StreamWriter writer, in byte initialTabsCount = 0) : base(initialTabsCount) => Writer = writer;
            public FileLogger(in string path, in byte initialTabsCount = 0) : this(new StreamWriter(path), initialTabsCount) { /* Left empty. */ }

#if CS5
            public FileLogger(in byte initialTabsCount = 0) : this(GetPath(), initialTabsCount) { /* Left empty. */ }

            private static string GetPath()
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), UtilHelpers.GetAssemblyName(), "logs");

                _ = Directory.CreateDirectory(path);

                return Path.Combine(path, $"{Guid.NewGuid()}.log");
            }
#endif

            public static Logger GetLogger(in StreamWriter writer, in byte initialTabsCount = 0) => new FileLogger(writer, initialTabsCount).WriteLineAndFlush;
            public static Logger GetLogger(in string path, in byte initialTabsCount = 0) => new FileLogger(path, initialTabsCount).WriteLineAndFlush;
#if CS5
            public static Logger GetLogger(in byte initialTabsCount = 0) => new FileLogger(initialTabsCount).WriteLineAndFlush;
#endif

            protected override void WriteLineOverride(string
#if CS8
                ?
#endif
                msg, LoggingLevel level) => Writer.WriteLine($"[{level.ToChar()}] {msg}");

            public void WriteLineAndFlush(string
#if CS8
                ?
#endif
                msg, bool? increment, LoggingLevel level)
            {
                WriteLine(msg, increment, level);

                Writer.Flush();
            }
        }

        public delegate void Logger(string
#if CS8
            ?
#endif
            message, bool? increment, LoggingLevel level);
    }

    public static class Console
    {
        public static unsafe void ReadLines(params IValueObject<string
#if CS8
                ?
#endif
                >[] @params)
        {
            ThrowIfNull(@params, nameof(@params));

            foreach (IValueObject<string
#if CS8
                ?
#endif
                >
#if CS8
                ?
#endif
                param in @params)
            {
                if (param == null)

                    throw new ArgumentException("The given array contains one or more null values.", nameof(@params));

                param.Value = System.Console.ReadLine();
            }
        }

        public static unsafe void ReadLines(params KeyValuePair<string, IValueObject<string
#if CS8
                ?
#endif
                >>[] @params)
        {
            ThrowIfNull(@params, nameof(@params));

            foreach (KeyValuePair<string, IValueObject<string
#if CS8
                ?
#endif
                >> param in @params)

                param.Value.Value = ReadLine(param.Key);
        }

        public static string
#if CS8
                ?
#endif
                ReadLine(in string msg)
        {
            WriteLine(msg);

            return System.Console.ReadLine();
        }

#if CS8
        [return: NotNull]
#endif
        public static T ReadLine<T>(in string msg, in Converter<string
#if CS8
                ?
#endif
                , T
#if CS9
                ?
#endif
                > converter, in string errorMessage)
        {
            T
#if CS9
            ?
#endif
                result;

            while ((result = converter(ReadLine(msg))) == null)

                WriteLine(errorMessage);

            return result;
        }

        public static T ReadValue<T>(in string msg, in Converter<string
#if CS8
                ?
#endif
                , T?> converter, in string errorMessage) where T : struct => ReadLine(msg, converter, errorMessage).Value;

        public static void WriteMenu<T>(in
#if CS5
            IReadOnlyList
#else
            List
#endif
            <KeyValuePair<string, T>> menu)
        {
            for (int i = 0; i < menu.Count; i++)

                WriteLine($"{i}: {menu[i].Key}");
        }

        public static void HandleMenu(
#if CS5
            IReadOnlyList
#else
            List
#endif
            <KeyValuePair<string, Func<bool>>> menu, in string msg, in bool clear)
        {
            do
            {
                if (clear)

                    Clear();

                if (!string.IsNullOrEmpty(msg))

                    WriteLine(msg);

                WriteMenu(menu);
            }

            while (menu[ReadValue<int>("Please make a choice to continue: ", s => int.TryParse(s, out int packageId) && packageId.Between(0, menu.Count - 1, true, true) ?
#if !CS9
            (int?)
#endif
            packageId : null, "Invalid value.")].Value());
        }

#if CS5
        public static void WriteMenu<T>(params KeyValuePair<string, T>[] menu) => WriteMenu(menu.AsFromType<IReadOnlyList<KeyValuePair<string, T>>>());

        public static void HandleMenu(in string msg, in bool clear, params KeyValuePair<string, Func<bool>>[] menu) => HandleMenu(menu.AsFromType<IReadOnlyList<KeyValuePair<string, Func<bool>>>>(), msg, clear);
#endif
    }
}
