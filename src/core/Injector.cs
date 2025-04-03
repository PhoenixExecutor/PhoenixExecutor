using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class RobloxInjector
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, 
        uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, 
        byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes,
        uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

    public bool Inject(int processId, string dllPath)
    {
        IntPtr hProcess = OpenProcess(0x1F0FFF, false, processId);
        if (hProcess == IntPtr.Zero) return false;

        IntPtr allocAddr = VirtualAllocEx(hProcess, IntPtr.Zero, 
            (uint)dllPath.Length, 0x1000 | 0x2000, 0x40);
        
        if (allocAddr == IntPtr.Zero) return false;

        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(dllPath);
        bool success = WriteProcessMemory(hProcess, allocAddr, bytes, 
            (uint)bytes.Length, out _);
        
        if (!success) return false;

        IntPtr loadLibAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
        IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, 
            loadLibAddr, allocAddr, 0, IntPtr.Zero);
        
        return hThread != IntPtr.Zero;
    }
}