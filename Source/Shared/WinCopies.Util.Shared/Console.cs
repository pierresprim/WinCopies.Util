using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
            System.Console.WriteLine(msg);

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

                System.Console.WriteLine(errorMessage);

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

                System.Console.WriteLine($"{i}: {menu[i].Key}");
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

                    System.Console.Clear();

                if (!string.IsNullOrEmpty(msg))

                    System.Console.WriteLine(msg);

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
