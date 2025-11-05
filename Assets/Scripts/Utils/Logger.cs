using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logger
{
    public static void NotImpl([CallerMemberName] string methodName = "")
    {
        Debug.LogWarning($"[{methodName}] 아직 구현 안 함.");
    }

    public static void LogWarning(string text, [CallerMemberName] string methodName = "")
    {
        Debug.LogWarning($"[{methodName}] {text}");
    }

    public static void Log(string text, [CallerMemberName] string methodName = "")
    {
        Debug.Log($"[{methodName}] {text}");
    }

    public static void LogError(string text, [CallerMemberName] string methodName = "")
    {
        Debug.LogError($"[{methodName}] {text}");
    }
}
