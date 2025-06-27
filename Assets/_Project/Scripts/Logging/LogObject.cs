using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace ColourMatch
{
    public class LogObject : MonoBehaviour
    {
        public Action<LogBundle> LogReceived;
        
        private Thread mainThread;
        private readonly Queue<LogBundle> threadedLogs = new();
        private readonly System.Object thisLock = new();
        private bool hasThreaded;

        public void Init()
        {
            mainThread = Thread.CurrentThread;
            Application.logMessageReceivedThreaded += OnLogReceived;
        }
        
        void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= OnLogReceived;
            lock (thisLock)
            {
                threadedLogs.Clear();
                hasThreaded = false;
            }
        }

        private void OnLogReceived(string condition, string stacktrace, LogType type)
        {
            var bundle = new LogBundle
            {
                Message = condition,
                StackTrace = stacktrace,
                Type = type
            };
            
            if (mainThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                OnLogReceived(bundle);
            }
            else
            {
                lock (thisLock)
                {
                    threadedLogs.Enqueue(bundle);
                    hasThreaded = true;
                }
            }
        }

        void Update()
        {
            if (hasThreaded)
            {
                lock (thisLock)
                {
                    while (threadedLogs.Any())
                    {
                        OnLogReceived(threadedLogs.Dequeue());
                    }

                    hasThreaded = false;
                }
            }
        }

        private void OnLogReceived(LogBundle bundle)
        {
            LogReceived?.Invoke(bundle);
        }
    }
}
