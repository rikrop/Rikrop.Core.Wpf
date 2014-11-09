using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rikrop.Core.Wpf.Helpers
{
    public static class ClipboardHelper
    {
        public static bool SetText(string text, int retryTimes = 10, int millisecondsRetryDelay = 100)
        {
            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    return true;
                }
                catch { }
                System.Threading.Thread.Sleep(millisecondsRetryDelay);
            }

            return false;
        }

        public static bool SetData(string format, object data, int retryTimes = 10, int millisecondsRetryDelay = 100)
        {
            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    Clipboard.SetData(format, data);
                    return true;
                }
                catch { }
                System.Threading.Thread.Sleep(millisecondsRetryDelay);
            }

            return false;
        }

        public static object GetData(string format, int retryTimes = 10, int millisecondsRetryDelay = 100)
        {
            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    return Clipboard.GetData(format);
                }
                catch { }
                System.Threading.Thread.Sleep(millisecondsRetryDelay);
            }

            return null;
        }
    }
}
