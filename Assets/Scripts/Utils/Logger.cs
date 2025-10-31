using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logger
{
    public static void NotImpl([CallerMemberName] string methodName = "")
    {
        Debug.LogWarning($"[{methodName}] 아직 구현 안 함.");
    }
}
