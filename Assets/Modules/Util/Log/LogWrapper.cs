namespace JSGCode.Util
{
    using System;
    using UnityEngine;

    public enum CustomLogType { Base, File, Util, Model, UI, Page, ChangeWorldState }

    public static class LogWrapper
    {
        const string BUILD_TARGET = "UNITY_ANDROID";

        private static string GetLogColor(CustomLogType logType) => logType switch
        {
            CustomLogType.UI => "#F8BBD0",
            CustomLogType.Page => "#BBDEFB",
            CustomLogType.ChangeWorldState => "#80CBC4",

            _ => "#" + ColorUtility.ToHtmlStringRGB(Color.white)
        };

        public static bool isDebugBuild
        {
            get { return Debug.isDebugBuild; }
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void Log(CustomLogType logType, object message)
        {
            Debug.LogFormat("<color={0}>{1} Log Message : {2}</color>", GetLogColor(logType), logType, message);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void Log(object message, UnityEngine.Object context)
        {
            Debug.Log(message, context);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogError(CustomLogType logType, object message)
        {
            Debug.LogErrorFormat("<color={0}>{1} Error Message : {2}</color>", GetLogColor(logType), logType, message);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogError(object message, UnityEngine.Object context)
        {
            Debug.LogError(message, context);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message.ToString());
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogWarning(CustomLogType logType, object message)
        {
            Debug.LogWarningFormat("<color={0}>{1} warning Message : {2}</color>", GetLogColor(logType), logType, message);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            Debug.LogWarning(message.ToString(), context);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            Debug.DrawRay(start, dir, color, duration, depthTest);
        }

        [System.Diagnostics.Conditional(BUILD_TARGET)]
        public static void Assert(bool condition)
        {
            if (!condition) throw new Exception();
        }
    }
}