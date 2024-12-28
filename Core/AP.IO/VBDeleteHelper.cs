using Microsoft.VisualBasic.FileIO;
using System;
using System.Reflection;
using AP.Reflection;

namespace AP.IO;

internal static class VBDeleteHelper
{        
    private static readonly Delegate _shellDelete;

    static VBDeleteHelper()
    {
        _shellDelete = typeof(Microsoft.VisualBasic.FileIO.FileSystem).GetMethod("ShellDelete", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).CreateDelegate();
    }

    internal static void DeleteFile(string fullName, bool permanent) => _shellDelete.DynamicInvoke(fullName, 4, permanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin, UICancelOption.DoNothing, 1);

    internal static void DeleteDirectory(string fullName, bool permanent) => _shellDelete.DynamicInvoke(fullName, 4, permanent ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin, UICancelOption.DoNothing, 2);
}
