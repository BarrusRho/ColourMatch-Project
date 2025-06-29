using System;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace ColourMatch
{
    public static class Logger
    {
        public const string PrefixSilent = "[SILENT] ";
        public const string PrefixSuperSilent = "[SUPER SILENT] ";
        public static Action<LogBundle> LogReceived;

        private static LogObject logObject;
        private static LogChannelsSO _logChannels;

        private static bool isInitialized;

        public static void SetChannels(LogChannelsSO logChannels)
        {
            _logChannels = logChannels;
        }

        public static void FirstTouch()
        {
            if (!isInitialized)
                Init();
        }

        private static void Init()
        {
            Assert.IsTrue(Thread.CurrentThread.ManagedThreadId == 1, "Log must be initialized on main thread");
            Clean();
            isInitialized = true;
            CreateLogObject();
            Application.quitting += Clean;
        }

        private static void CreateLogObject()
        {
            var go = new GameObject("[LogObject]");
            logObject = go.AddComponent<LogObject>();
            Object.DontDestroyOnLoad(logObject);
            logObject.LogReceived += log => LogReceived?.Invoke(log);
            logObject.Init();
        }

        private static void Clean()
        {
            isInitialized = false;
            Application.quitting -= Clean;
            LogReceived = null;
            if (logObject != null)
                Object.Destroy(logObject.gameObject);
        }

        public static void Warning<T>(T context, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Warning, context, message, channel);
        }

        public static void Warning(string typeName, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Warning,typeName, message, channel);
        }
        
        public static void Error<T>(T context, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, context, message, channel);
        }
        
        public static void Error(string typeName, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, typeName, message, channel);
        }
        
        public static void SilentError<T>(T context, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, context, PrefixSilent + message, channel);
        }
        
        public static void SilentError(string typeName, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, typeName, PrefixSilent + message, channel);
        }
        
        public static void SuperSilentError<T>(T context, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, context, PrefixSuperSilent + message, channel);
        }
        
        public static void SuperSilentError(string typeName, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Error, typeName, PrefixSuperSilent + message, channel);
        }
        
        public static void BasicLog<T>(T context, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Log, context, message, channel);
        }
        
        public static void BasicLog(string typeName, string message, LogChannel channel = LogChannel.None)
        {
            LogInternal(LogType.Log,typeName,  message, channel);
        }

        private static void LogInternal<T>(LogType type, T context, string message, LogChannel channel = LogChannel.None)
        {
            ConvertToUnityLogFormat(type, typeof(T).Name, message, channel, context);
        }
        
        private static string GetColor(LogChannel channel) => channel switch
        {
            LogChannel.None => "cyan",
            LogChannel.BasicLog => "black",
            LogChannel.Authentification => "magenta",
            LogChannel.Services => "green",
            LogChannel.Audio => "yellow",
            LogChannel.Gameplay => "orange",
            LogChannel.PoolingService => "#FFC0CB",
            LogChannel.Events => "blue",
            LogChannel.UI => "white",
            _ => "red"
        };

        private static void ConvertToUnityLogFormat(LogType type, string typeName, string message, LogChannel channel, object context = null)
        {
            message = RemoveCurlyBrackets(message);

            if (channel == LogChannel.None || (_logChannels != null && _logChannels.LogChannels.Contains(channel)))
            {
                if (channel != LogChannel.None)
                    message = $"<color={GetColor(channel)}>[{channel}]</color> {message}";

                if (context is UnityEngine.Object unityObj)
                    Debug.LogFormat(type, LogOption.None, unityObj, message);
                else
                    Debug.LogFormat(type, LogOption.None, null, message);
            }
        }

        private static string RemoveCurlyBrackets(string message)
        {
            return Regex.Replace(message, "[{}]", "");
        }
    }
}
