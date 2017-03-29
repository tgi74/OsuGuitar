using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OsuGuitar
{
    internal static class OSLink
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        const int KEY_DOWN_EVENT = 0x0001; //Key down flag
        const int KEY_UP_EVENT = 0x0002; //Key up flag

        public static void DownKey(byte key)
        {
            keybd_event(key, 0, KEY_DOWN_EVENT, 0);
        }

        private static void UpKey(byte key)
        {
            keybd_event(key, 0, KEY_UP_EVENT, 0);
        }

        private static List<Keys> oldInputs;

        internal static void UpdateInputs(List<Keys> keys)
        {
            foreach (var k in keys)
                DownKey((byte)k);

            if (oldInputs != null)
                foreach (var k in oldInputs)
                    if (!keys.Contains(k))
                        UpKey((byte)k);
            
            oldInputs = keys;
        }

    }
}
