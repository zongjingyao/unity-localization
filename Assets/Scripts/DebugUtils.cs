using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DebugUtils 
{
    public static bool s_bIsDebug = true;

    public static void Log(object message)
    {
        Log(message,null);
    }

    public static void Log(object message, Color color)
    {
        Log(message, null, color);
    }

    public static void Log(object message, Object context)
    {
        if (s_bIsDebug)
        {
            UnityEngine.Debug.Log(message, context);
        }
    }

    public static void Log(object message, Object context, Color color)
    {
        if (s_bIsDebug)
        {
#if UNITY_EDITOR
            string strColor = ColorUtility.ToHtmlStringRGB(color);
            string strMsg = "<color=#" + strColor + ">" + message + "</color>";
            UnityEngine.Debug.Log(strMsg, context);
#else
            UnityEngine.Debug.Log(message, context);
#endif
        }
    }

    static public void LogError(object message)
    {
        LogError(message, null);
    }

    static public void LogError(object message, Object context)
    {
        if (s_bIsDebug)
        {
            UnityEngine.Debug.LogError(message, context);
        }
    }

    static public void LogWarning(object message)
    {
        LogWarning(message, null);
    }

    static public void LogWarning(object message, Object context)
    {
        if (s_bIsDebug)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
    }

    static public void LogFileAndLine(string msg = null)
    {
        if (s_bIsDebug)
        {
            StackTrace insStackTrace = new StackTrace(true);
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            string fileName = insStackFrame.GetFileName();
            fileName = fileName.Substring(fileName.IndexOf("Assets") + 7);
            string debugInfo = "File: " + fileName + "\n"
                + "Method: " + insStackFrame.GetMethod() + "\n"
                + "Line: " + insStackFrame.GetFileLineNumber();
            if (msg != null)
            {
                debugInfo = "Message: " + msg + "\n" + debugInfo;
            }
            Log(debugInfo);
        }
    }
}
