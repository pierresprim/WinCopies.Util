/* Copyright © Pierre Sprimont, 2019
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using System;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public static class Clipboard
    {
        private static void ThrowOnInvalidSleepTime(in int sleepTime)
        {
            if (sleepTime < 0)

                throw new ArgumentOutOfRangeException(nameof(sleepTime), sleepTime, $"{nameof(sleepTime)} must be greater or equal to zero.");
        }

        #region File Drop List
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in System.Collections.Specialized.StringCollection fileDropList)
        {
            try
            {
                System.Windows.Clipboard.SetFileDropList(fileDropList);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in System.Collections.Specialized.StringCollection fileDropList, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(fileDropList)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in System.Collections.Specialized.StringCollection fileDropList, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(fileDropList)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion

        #region Text
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in string text)
        {
            try
            {
                System.Windows.Clipboard.SetText(text);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in string text, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(text)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in string text, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(text)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }



        public static bool TrySetClipboardOnSuccessHResult(in string text, in TextDataFormat format)
        {
            try
            {
                System.Windows.Clipboard.SetText(text, format);

                return true;
            }

            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in string text, in TextDataFormat format, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(text, format)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in string text, in TextDataFormat format, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(text, format)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion

        #region Audio
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in byte[] audioBytes)
        {
            try
            {
                System.Windows.Clipboard.SetAudio(audioBytes);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in byte[] audioBytes, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(audioBytes)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in byte[] audioBytes, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(audioBytes)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }



        public static bool TrySetClipboardOnSuccessHResult(in System.IO.Stream audioStream)
        {
            try
            {
                System.Windows.Clipboard.SetAudio(audioStream);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in System.IO.Stream audioStream, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(audioStream)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in System.IO.Stream audioStream, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(audioStream)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion

        #region Image
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in BitmapSource image)
        {
            try
            {
                System.Windows.Clipboard.SetImage(image);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in BitmapSource image, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(image)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in BitmapSource image, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(image)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion

        #region Data
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in string format, in object data)
        {
            try
            {
                System.Windows.Clipboard.SetData(format, data);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in string format, in object data, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(format, data)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in string format, in object data, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(format, data)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion

        #region Data Object
#if CS6
        public static bool TrySetClipboardOnSuccessHResult(in object data)
        {
            try
            {
                System.Windows.Clipboard.SetDataObject(data);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in object data, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(data)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in object data, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(data)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }



        public static bool TrySetClipboardOnSuccessHResult(in object data, in bool copy)
        {
            try
            {
                System.Windows.Clipboard.SetDataObject(data, copy);

                return true;
            }
            catch (Exception ex) when (ex.HResult >= 0)
            {
                return false;
            }
        }

        public static bool TrySetClipboardOnSuccessHResult(in object data, in bool copy, in double maxMilliSeconds)
        {
            if (maxMilliSeconds == 0)

                return false;

            DateTime start = DateTime.Now;

            do

                if (TrySetClipboardOnSuccessHResult(data, copy)) return true;

            while ((DateTime.Now - start).TotalMilliseconds < maxMilliSeconds);

            return false;
        }

        public static bool TrySetClipboardOnSuccessHResult(in object data, in bool copy, in int sleepTime, in uint tryCount)
        {
            ThrowOnInvalidSleepTime(sleepTime);

            for (uint i = 0; i < tryCount; i++)
            {
                if (TrySetClipboardOnSuccessHResult(data, copy)) return true;

                Thread.Sleep(sleepTime);
            }

            return false;
        }
#endif
        #endregion
    }
}
