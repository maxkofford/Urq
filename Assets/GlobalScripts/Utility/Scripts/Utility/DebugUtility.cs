//#define LOG_EVERYTHINGUTILITIES_ASSERTIONS
#if UNITY_EDITOR
#define LOG_EVERYTHING
#endif
#if LOG_EVERYTHING
#define UTILITIES_ASSERTIONS
#define UTILITIES_LOG_ASSERTIONS
#define UTILITIES_LOG_ERRORS
#define UTILITIES_LOG_MESSAGES
#define UTILITIES_LOG_WARNINGS
#elif (DEBUG || DEVELOPMENT_BUILD)
#define UTILITIES_ASSERTIONS
#define UTILITIES_LOG_ASSERTIONS
#define UTILITIES_LOG_ERRORS
#define UTILITIES_LOG_WARNINGS
#else
#define UTILITIES_LOG_ERRORS
#endif

using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utilities
{
	using Object = UnityEngine.Object;

	public static class Debug
	{
		public static bool developerConsoleVisible {
			get { return UnityEngine.Debug.developerConsoleVisible; }
			set { UnityEngine.Debug.developerConsoleVisible = value; }
		}

		public static bool isDebugBuild { get { return UnityEngine.Debug.isDebugBuild; } }
		public static ILogger logger { get { return UnityEngine.Debug.logger; } }

		// Since "Conditional" attributes are evaluated at the call point we use #if here to force the issue globally.
		// You should never define UTILITIES_UNDEFINED so those calls never happen.

        private static bool ConditionalBreakpoint(bool condition)
        {
            // Make it easy to debug these things and set breakpoints.
            if (!condition)
            {
                return false;
            }
            return true;
        }

#if UTILITIES_ASSERTIONS
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition)); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition), message); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition), message); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, Object context) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition), context); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, string message, Object context) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition), message, context); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void Assert(bool condition, object message, Object context) { UnityEngine.Debug.Assert(ConditionalBreakpoint(condition), message, context); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, string format, params object[] args) { UnityEngine.Debug.AssertFormat(ConditionalBreakpoint(condition), format, args); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void AssertFormat(bool condition, Object context, string format, params object[] args) { UnityEngine.Debug.AssertFormat(ConditionalBreakpoint(condition), context, format, args); }
#else
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition, string message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition, object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition, string message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Assert(bool condition, object message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void AssertFormat(bool condition, string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void AssertFormat(bool condition, Object context, string format, params object[] args) { }
#endif

#if UTILITIES_LOG_ASSERTIONS
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message) { UnityEngine.Debug.LogAssertion(message); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertion(object message, Object context) { UnityEngine.Debug.LogAssertion(message, context); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(string format, params object[] args) { UnityEngine.Debug.LogAssertionFormat(format, args); }
		[Conditional("UNITY_ASSERTIONS")]
		public static void LogAssertionFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogAssertionFormat(context, format, args); }
#else
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogAssertion(object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogAssertion(object message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogAssertionFormat(string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogAssertionFormat(Object context, string format, params object[] args) { }
#endif

#if UTILITIES_LOG_ERRORS
		public static void LogError(object message) { UnityEngine.Debug.LogError(message); }
		public static void LogError(object message, Object context) { UnityEngine.Debug.LogError(message, context); }
		public static void LogErrorFormat(string format, params object[] args) { UnityEngine.Debug.LogErrorFormat(format, args); }
		public static void LogErrorFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogErrorFormat(context, format, args); }
        // The system implementation sucks. It only reports the exception type.
        //public static void LogException(System.Exception exception) { UnityEngine.Debug.LogException(exception); }
        //public static void LogException(System.Exception exception, Object context) { UnityEngine.Debug.LogException(exception, context); }
        public static void LogException(System.Exception exception) { LogErrorFormat("{0}: {1} {2}\n{3}", exception.GetType().Name, exception.Message, exception.ToString(), exception.StackTrace); }
        public static void LogException(System.Exception exception, Object context) { LogErrorFormat(context, "{0}: {1} {2}\n{3}", exception.GetType().Name, exception.Message, exception.ToString(), exception.StackTrace); }
#else
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogError(object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogError(object message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogErrorFormat(string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogErrorFormat(Object context, string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogException(System.Exception exception) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogException(System.Exception exception, Object context) { }
#endif

#if UTILITIES_LOG_MESSAGES
        public static void Log(object message) { UnityEngine.Debug.Log(message); }
		public static void Log(object message, Object context) { UnityEngine.Debug.Log(message, context); }

		public static void LogFormat(string format, params object[] args) { UnityEngine.Debug.LogFormat(format, args); }
		public static void LogFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogFormat(context, format, args); }

		public static void LogIf(bool condition, object message) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.Log(message); }
		public static void LogIf(bool condition, object message, Object context) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.Log(message, context); }

		public static void LogFormatIf(bool condition, string format, params object[] args) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogFormat(format, args); }
		public static void LogFormatIf(bool condition, Object context, string format, params object[] args) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogFormat(context, format, args); }
#else
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Log(object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void Log(object message, Object context) { }

		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogFormat(string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogFormat(Object context, string format, params object[] args) { }

		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogIf(bool condition, object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogIf(bool condition, object message, Object context) { }

		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogFormatIf(bool condition, string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogFormatIf(bool condition, Object context, string format, params object[] args) { }
#endif

#if UTILITIES_LOG_WARNINGS
        public static void LogWarning(object message) { UnityEngine.Debug.LogWarning(message); }
		public static void LogWarning(object message, Object context) { UnityEngine.Debug.LogWarning(message, context); }
		public static void LogWarningFormat(string format, params object[] args) { UnityEngine.Debug.LogWarningFormat(format, args); }
		public static void LogWarningFormat(Object context, string format, params object[] args) { UnityEngine.Debug.LogWarningFormat(context, format, args); }

		public static void LogWarningIf(bool condition, object message) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogWarning(message); }
		public static void LogWarningIf(bool condition, object message, Object context) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogWarning(message, context); }
		public static void LogWarningFormatIf(bool condition, string format, params object[] args) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogWarningFormat(format, args); }
		public static void LogWarningFormatIf(bool condition, Object context, string format, params object[] args) { if (ConditionalBreakpoint(condition)) UnityEngine.Debug.LogWarningFormat(context, format, args); }
#else
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarning(object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarning(object message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningFormat(string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningFormat(Object context, string format, params object[] args) { }
		
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningIf(bool condition, object message) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningIf(bool condition, object message, Object context) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningFormatIf(bool condition, string format, params object[] args) { }
		[Conditional("UTILITIES_UNDEFINED")]
		public static void LogWarningFormatIf(bool condition, Object context, string format, params object[] args) { }
#endif
	}
}
