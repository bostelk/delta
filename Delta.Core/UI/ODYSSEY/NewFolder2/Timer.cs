//using System;
//using System.Runtime.InteropServices;
//using System.Security;

//namespace AvengersUTD.Odyssey.UserInterface.Helpers
//{
//    /// <summary>
//    /// High Precision Timer Class
//    /// </summary>
//    public class Timer
//    {
//        long ticksPerSecond = 0;
//        long elapsedTime = 0;
//        long baseTime = 0;

//        /// <summary>
//        /// High Precision Timer Class
//        /// </summary>
//        public Timer()
//        {
//            // Use QueryPerformanceFrequency to get frequency of the timer
//            if (!QueryPerformanceFrequency(ref ticksPerSecond))
//                throw new ApplicationException("Timer: Performance Frequency Unavailable");
//            Reset();
//        }

//        [SuppressUnmanagedCodeSecurity]
//        [DllImport("kernel32")]
//        static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

//        [SuppressUnmanagedCodeSecurity]
//        [DllImport("kernel32")]
//        static extern bool QueryPerformanceCounter(ref long PerformanceCount);

//        /// <summary>
//        /// Resets the Timer, updates the base time
//        /// </summary>
//        public void Reset()
//        {
//            long time = 0;
//            QueryPerformanceCounter(ref time);
//            baseTime = time;
//            elapsedTime = 0;
//        }

//        /// <summary>
//        /// Get the time since last reset
//        /// </summary>
//        /// <returns>The time since last reset.</returns>
//        public double GetTime()
//        {
//            long time = 0;
//            QueryPerformanceCounter(ref time);
//            return (double) (time - baseTime)/(double) ticksPerSecond;
//        }

//        /// <summary>
//        /// Get the current time of the system
//        /// </summary>
//        /// <returns>The current time in seconds.</returns>
//        public double GetAbsoluteTime()
//        {
//            long time = 0;
//            QueryPerformanceCounter(ref time);
//            return (double) time/(double) ticksPerSecond;
//        }

//        /// <summary>
//        /// Get the time since last call of this method, This is a Rendering Timer
//        /// </summary>
//        /// <returns>The number of seconds since last call of this function.</returns>
//        public double GetElapsedTime()
//        {
//            long time = 0;
//            QueryPerformanceCounter(ref time);
//            double absoluteTime = (double) (time - elapsedTime)/(double) ticksPerSecond;
//            elapsedTime = time;
//            return absoluteTime;
//        }
//    }
//}