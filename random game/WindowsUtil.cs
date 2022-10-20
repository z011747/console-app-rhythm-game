using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using System.Runtime.InteropServices; //allow imports

namespace random_game
{
    class WindowsUtil
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        //https://stackoverflow.com/questions/31271828/how-do-i-use-setwindowpos-to-center-a-window?noredirect=1&lq=1
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow(); //for getting handle

        const int SWP_NOMOVE = 0X2;
        const int SWP_NOSIZE = 1;
        const int SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;


        public static int windowX { get; private set; }
        public static int windowY { get; private set; }


        public static void centerScreen()
        {
            int resWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int resHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                RECT screenRect;
                GetWindowRect(handle, out screenRect);
                int winWidth = screenRect.Right - screenRect.Left;
                int centerPosX = (int)((resWidth - winWidth) * 0.5);
                int winHeight = screenRect.Bottom - screenRect.Top;
                int centerPosY = (int)((resHeight - winHeight) * 0.5);

                windowX = centerPosX;
                windowY = centerPosY;

                SetWindowPos(handle, handle, centerPosX, centerPosY, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        const int LWA_COLORKEY = 0x00000001;
        const int LWA_ALPHA = 0x00000002;

        //https://stackoverflow.com/questions/1360758/modifying-opacity-of-any-window-from-c-sharp
        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);






        public static void setWindowAlpha(float alpha)
        {
            int byteAlpha = (int)Math.Round(alpha * 255);
            byteAlpha = MathUtil.bound(byteAlpha, 0, 255);
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                SetWindowLong(handle, GWL_EXSTYLE, GetWindowLong(handle, GWL_EXSTYLE));
                SetLayeredWindowAttributes(handle, 0xFFFFFFFF, (byte)byteAlpha, LWA_COLORKEY | LWA_ALPHA);
            }
        }


        public static void setWindowPos(int x, int y)
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                windowX = x;
                windowY = y;
                SetWindowPos(handle, handle, x, y, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
            }
        }

        public static int getScreenResolutionWidth()
        {
            return System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        }

        public static int getScreenResolutionHeight()
        {
            return System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        }

        public static int getWindowWidth()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                RECT screenRect;
                GetWindowRect(handle, out screenRect);
                return screenRect.Right - screenRect.Left;
            }
            return 0;
        }

        public static int getWindowHeight()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                RECT screenRect;
                GetWindowRect(handle, out screenRect);
                return screenRect.Bottom - screenRect.Top;
            }
            return 0;
        }


        public static bool isWindowFocused() //doesnt even work
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                return true;
            }
            return false;
        }
    }
}
