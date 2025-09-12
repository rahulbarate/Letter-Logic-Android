using System.Diagnostics;
using UnityEngine;

public static class CustomLogger
{
    private static int logCount = 0;

    public static void Log(object message)
    {
        logCount++;

        // Get the stack frame that called this method (1 step up)
        StackFrame frame = new StackTrace(1, true).GetFrame(0);
        var method = frame.GetMethod();
        string className = method.DeclaringType.Name;
        string methodName = method.Name;
        int lineNumber = frame.GetFileLineNumber();

        string fullMessage = $"<color=#4DA8DA>[Log {logCount}]</color> " +
                             $"<b>{className}.{methodName}()</b> " +
                             $"<i>(Line {lineNumber})</i> ➤ {message?.ToString()}";

        UnityEngine.Debug.Log(fullMessage);
    }

    public static void LogWarning(object message)
    {
        logCount++;
        StackFrame frame = new StackTrace(1, true).GetFrame(0);
        string className = frame.GetMethod().DeclaringType.Name;
        string methodName = frame.GetMethod().Name;
        int lineNumber = frame.GetFileLineNumber();

        string fullMessage = $"<color=orange>[Warning {logCount}]</color> " +
                             $"<b>{className}.{methodName}()</b> " +
                             $"<i>(Line {lineNumber})</i> ➤ {message?.ToString()}";

        UnityEngine.Debug.LogWarning(fullMessage);
    }

    public static void LogError(object message)
    {
        logCount++;
        StackFrame frame = new StackTrace(1, true).GetFrame(0);
        string className = frame.GetMethod().DeclaringType.Name;
        string methodName = frame.GetMethod().Name;
        int lineNumber = frame.GetFileLineNumber();

        string fullMessage = $"<color=red>[Error {logCount}]</color> " +
                             $"<b>{className}.{methodName}()</b> " +
                             $"<i>(Line {lineNumber})</i> ➤ {message?.ToString()}";

        UnityEngine.Debug.LogError(fullMessage);
    }

    public static void ResetCount()
    {
        logCount = 0;
    }
}
