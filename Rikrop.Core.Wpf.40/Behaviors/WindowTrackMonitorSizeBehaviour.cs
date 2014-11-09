using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Rikrop.Core.Wpf.Behaviors
{
    public class WindowTrackMonitorSizeBehaviour
    {
        public static readonly DependencyProperty TrackMonitorSizeProperty =
            DependencyProperty.RegisterAttached("TrackMonitorSize", typeof (bool), typeof (WindowTrackMonitorSizeBehaviour),
                                                new PropertyMetadata(false, TrackMonitorSizePropertyChanged));

        public static void SetTrackMonitorSize(DependencyObject dp, bool value)
        {
            dp.SetValue(TrackMonitorSizeProperty, value);
        }

        public static bool GetTrackMonitorSize(DependencyObject dp)
        {
            return (bool) dp.GetValue(TrackMonitorSizeProperty);
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        private static void TrackMonitorSizePropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs dargs)
        {
            var w = dobj as Window;
            if (w == null)
            {
                return;
            }
            if ((bool) dargs.NewValue)
            {
                w.SourceInitialized += WindowOnSourceInitialized;
            }
            else
            {
                w.SourceInitialized -= WindowOnSourceInitialized;
            }
        }

        private static void WindowOnSourceInitialized(object sender, EventArgs eventArgs)
        {
            var handle = (new WindowInteropHelper((Window) sender)).Handle;
            HwndSource.FromHwnd(handle).AddHook(WindowProc);
        }

        private static IntPtr WindowProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return (IntPtr) 0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            var MONITOR_DEFAULTTONEAREST = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>
            public int cbSize = Marshal.SizeOf(typeof (MONITORINFO));

            /// <summary>
            /// </summary>
            public RECT rcMonitor;

            /// <summary>
            /// </summary>
            public RECT rcWork;

            /// <summary>
            /// </summary>
            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            ///     x coordinate of point.
            /// </summary>
            public int x;

            /// <summary>
            ///     y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            ///     Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary>
            ///     Win32
            /// </summary>
            public int left;

            /// <summary>
            ///     Win32
            /// </summary>
            public int top;

            /// <summary>
            ///     Win32
            /// </summary>
            public int right;

            /// <summary>
            ///     Win32
            /// </summary>
            public int bottom;

            /// <summary>
            ///     Win32
            /// </summary>
            public static readonly RECT Empty;

            /// <summary>
            ///     Win32
            /// </summary>
            public int Width
            {
                get { return Math.Abs(right - left); } // Abs needed for BIDI OS
            }

            /// <summary>
            ///     Win32
            /// </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary>
            ///     Win32
            /// </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary>
            ///     Win32
            /// </summary>
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }

            /// <summary>
            ///     Win32
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }

            /// <summary>
            ///     Return a user friendly representation of this struct
            /// </summary>
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary>
            ///     Determine if 2 RECT are equal (deep compare)
            /// </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }
                return (this == (RECT) obj);
            }

            /// <summary>
            ///     Return the HashCode for this struct (not garanteed to be unique)
            /// </summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary>
            ///     Determine if 2 RECT are equal (deep compare)
            /// </summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary>
            ///     Determine if 2 RECT are different(deep compare)
            /// </summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }
    }
}