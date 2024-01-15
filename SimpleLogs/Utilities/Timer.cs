using System;
using System.Diagnostics;

namespace SimpleLogs.Utilities
{
    public class Timer
    {
        private Stopwatch stopwatch;

        public Timer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public TimeSpan GetElapsedTime()
        {
            return stopwatch.Elapsed;
        }
    }
}