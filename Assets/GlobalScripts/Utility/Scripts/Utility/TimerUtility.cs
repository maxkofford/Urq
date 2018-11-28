namespace Utilities
{
    using System;
    using UnityEngine;

    // Note the following before you use TickCount for limiting frame time used:
    // 1) We only have 10-11 ms *in-total* for each frame at 90 fps, so you can't expect to take anywhere near
    //    that much time and leave anything useful for the many other functions.
    // 2) Environment.TickCount is based on the GetTickCount() WinAPI function. It's in milliseconds but
    //    the actual precision of it is between 10-16 ms. So you can't measure shorter time intervals (or you'll get 0)
    //    (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms724408%28v=vs.85%29.aspx)

    /// <summary>
    /// If we standardize on this CoroutineTimer value type it'll pass around efficiently and guarantee
    /// sufficient precision with safeguards that we aren't hogging the main thread from a coroutine.
    /// Let's update it as we find it needs work and features.
    /// </summary>
    struct CoroutineTimer // : Timers.ICoroutineInterval
    {
        
        // I'm just a little concerned about the potential for boxing and performance problems when declaring an interface with a struct value type.
        public const float kDefaultInterval = 8f; // milliseconds - this probably needs to be tuned 
        public const float kMaximumUsedRatioOfTimeLeft = 1f; // ratio - this may need tuning too

        private Timers.ICoroutineInterval m_timer;

        public CoroutineTimer(float intervalMilliseconds = CoroutineTimer.kDefaultInterval, bool start = true, Timers.ICoroutineInterval timer = null)
        {
            if (null == timer) {
                timer = new Timers.StopwatchTimer(intervalMilliseconds, start);
            }
            m_timer = timer;
        }

        public bool ShouldYield(bool reset = true)
        {
            return m_timer.ShouldYield(reset);
        }

        public void Yielded()
        {
            m_timer.Yielded();
        }
    }

    namespace Timers
    {
        public enum theTimers {Stopwatch,System,EnvTick }

        public class DefaultTimer
        {
            public static theTimers defaultTimer = theTimers.Stopwatch;

            public static ICoroutineInterval GetDefault()
            {
                return GetTimer(defaultTimer);
            }
            
            public static ICoroutineInterval GetTimer(theTimers t, float intervalMilliseconds = CoroutineTimer.kDefaultInterval, bool start = true)
            {
                ICoroutineInterval theTimer = null;
                switch (t)
                {
                    case (theTimers.Stopwatch):
                        theTimer = new Timers.StopwatchTimer(intervalMilliseconds,start);
                        break;
                    case (theTimers.System):
                        theTimer = new Timers.SystemTimer(intervalMilliseconds, start);
                        break;
                    case (theTimers.EnvTick):
                        theTimer = new Timers.EnvTickCountTimer(intervalMilliseconds, start);
                        break;
                }
                return theTimer;
            }
        }

        public interface ICoroutineInterval
        {
            bool ShouldYield(bool reset = true);
            void Yielded();
        }

        /// <summary>
        /// This class seems to promise decent performance and precision.
        /// I think it's the better choice until we measure a problem.
        /// </summary>
        public class StopwatchTimer : System.Diagnostics.Stopwatch, ICoroutineInterval
        {
            private float m_maxInterval;
            private float m_timeLimit;
            private int m_lastFrame;

#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            private static bool g_reported = false;
#endif

            public StopwatchTimer(float intervalMilliseconds = CoroutineTimer.kDefaultInterval, bool start = true)
                : base()
            {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                if (!g_reported)
                {
                    g_reported = true;

                    // Display the timer frequency and resolution.
                    if (IsHighResolution)
                    {
                        Debug.Log("StopwatchTimer: operations timed using the system's high-resolution performance counter.");
                    }
                    else
                    {
                        Debug.Log("StopwatchTimer: operations timed using the DateTime class.");
                    }

                    long frequency = Frequency;
                    Debug.LogFormat("StopwatchTimer:  timer frequency in ticks per second = {0}", frequency);
                    long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
                    Debug.LogFormat("StopwatchTimer:  timer is accurate within {0} nanoseconds ({1} milliseconds)",
                        nanosecPerTick, nanosecPerTick * 1.0e-6f);
                }
#endif
                //maximum delta time is in seconds >.< and is kinda useless for our purposes because its returning 30 milliseconds of frame time but thats way too high for vr's 90 fps (we need 15 or lower)
                m_maxInterval = Mathf.Min(intervalMilliseconds, Time.maximumDeltaTime*1000 );
                this.ResetForFrame(start);
            }

            public bool ShouldYield(bool reset = true)
            {
                // If we've just entered a new frame we really need to reset.
                if (m_lastFrame != Time.frameCount)
                {
                    this.ResetForFrame(true);
                    reset = false;
                }

                // This test means we'll (probably) always skip yielding immediately after a reset.
                bool result = (this.ElapsedMilliseconds > m_maxInterval);               
                if (result && reset)
                {
                    // Don't really want to start if we're just going to yield, but we can't assume the caller will start it later.
                    this.ResetForFrame(true);
                }
                return result;
            }

            public void Yielded()
            {
                this.ResetForFrame(true);
            }

            private void ResetForFrame(bool start)
            {
                
                //i think this was supposed to be calculating the amount of time since the current frame started
                //unfortunantly this doesnt actually do that because Time.realtimeSinceStartup can and does get much higher then Time.time (cumulative increasing over every frame)
                //var timeSoFar = Time.realtimeSinceStartup - Time.time;
                //var timeLeft = Time.maximumDeltaTime - timeSoFar;
                //m_timeLimit = Mathf.Min(timeLeft * CoroutineTimer.kMaximumUsedRatioOfTimeLeft, m_maxInterval);

                m_lastFrame = Time.frameCount;
                this.Reset();
                if (start)
                {
                    this.Start();
                }
            }
        }

        /// <summary>
        /// I really don't love this class. The problems are:
        ///   1) no correlation between the frame start time and the event trigger
        ///   2) the timer runs even when we aren't in our scripts
        ///   3) coroutines kind of suck in a 70's - 90's cooperative threading way :(
        ///      Still, couroutines are the way of life with Unity.
        /// </summary>
        public class SystemTimer : System.Timers.Timer, ICoroutineInterval
        {
            private bool m_shouldYield = false;

            public SystemTimer(Double intervalMilliseconds = CoroutineTimer.kDefaultInterval, bool start = true)
                : base(intervalMilliseconds)
            {
                //m_shouldYield = initiallyYield;
                base.AutoReset = true;
                base.Elapsed += IntervalReached;
                if (start) {
                    base.Start();
                }
            }

            public bool ShouldYield(bool reset = true)
            {
                bool result = m_shouldYield;
                m_shouldYield = (result & (!reset));
                return result;
            }

            public void Yielded()
            {
                // It might be nice to reset the timer interval but I don't know how costly that is.
                m_shouldYield = false;
            }

            private void IntervalReached(object sender, System.Timers.ElapsedEventArgs e)
            {
                m_shouldYield = true;
            }
        }


        /// <summary>
        /// Adding this just for consistency even if we never use it.
        /// </summary>
        public class EnvTickCountTimer : ICoroutineInterval
        {
            private int frameStart;
            private float maxFrameTime;
            public EnvTickCountTimer(float intervalMilliseconds = CoroutineTimer.kDefaultInterval, bool start = true)
            {
                frameStart = Environment.TickCount;
                maxFrameTime = intervalMilliseconds;

            }

            public bool ShouldYield(bool reset = true)
            {
                if (frameStart + maxFrameTime * CoroutineTimer.kMaximumUsedRatioOfTimeLeft < Environment.TickCount)
                {

                    if(reset)
                        frameStart = Environment.TickCount;

                    return true;

                }
                return false;
            }

            public void Yielded()
            {
                frameStart = Environment.TickCount;
            }
        }
    }
}
