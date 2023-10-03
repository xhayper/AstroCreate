using System;
using System.Runtime.InteropServices;

namespace AstroDX.BASS.Native;

public static class Kernel32
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr LoadLibrary(string libname);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool FreeLibrary(IntPtr hModule);
}