namespace Utilities
{

    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.Assertions;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    // Even though they are used like normal methods, extension
    // methods must be declared static. Notice that the first
    // parameter has the 'this' keyword followed by a variable
    // whose type denotes which class the extension method
    // becomes a part of.

    public static partial class UnityExtensionUtility
    {
        #region general generic utility methods

        /// <summary>
        /// A minor shortcut for setting field defaults without having to enter the type again.
        /// </summary>
        public static void SetDefault<T, U>(this T _, out U obj)
        {
            obj = default(U);
        }

        #endregion

        #region general object/GameObject methods

        /// <summary>
        /// Checks if an object has been destroyed.
        /// This implementation was buggy, and now I'm unsure whether it's wise to have it at all. But now it should work.
        /// </summary>
        /// <param name="obj">Object reference to check for destructedness</param>
        /// <returns>If the object has been marked as destroyed by UnityEngine</returns>
        [System.Obsolete("This implementation was buggy, and now I'm unsure whether it's wise to have it at all. But now it should work.")]
        public static bool IsDestroyed(this Object /*GameObject*/ obj)
        {
            // UnityEngine overloads the == operator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return obj == null || ReferenceEquals(obj, null);
        }

        /// <summary>
        /// Gets or adds a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.gameObject.AddComponent<T>();
            }
            return result;
        }
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T result = go.GetComponent<T>();
            if (result == null)
            {
                result = go.AddComponent<T>();
            }
            return result;
        }

        public static T CopyFrom<T>(this Component comp, T other) where T : Component
        {
            System.Type type = comp.GetType();
            if (null == other || type != other.GetType())
                return null; // type mismatch

            BindingFlags flags = (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly);
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static T AddComponent<T>(this GameObject go, T prototype) where T : Component
        {
            System.Type type = typeof(T);
            if (null == prototype || type != prototype.GetType())
                return null; // type mismatch

            return go.AddComponent<T>().CopyFrom(prototype);
        }

        public static T AddComponent<T>(this GameObject go, GameObject prototype) where T : Component
        {
            return AddComponent<T>(go, prototype.GetComponent<T>());
        }

        #endregion

#if (UNITY_METRO && !UNITY_EDITOR)
    public static System.Type[] FindInterfaces(this System.Reflection.TypeInfo self, System.Reflection.TypeFilter filter, object filterCriteria)
    {
        var result = new List<System.Type>();
        foreach (var ii in self.ImplementedInterfaces)
        {
            if (filter(ii, filterCriteria))
            {
                result.Add(ii);
            }
        }
        return result.ToArray();
    }
#endif
    }

    namespace Strata
    {
#if UNITY_WSA
    // A the moment I only have a HoloLens implementation for this.
    using ThreadTask = UnityEngine.WSA.AppCallbackItem;
#endif

        public static partial class Util
        {
            public static void DestroyIf<T>(T obj) where T : Object
            {
                if (null != obj)
                {
                    Object.Destroy(obj);
                }
            }
            public static void DestroyIf<T>(T obj, float t) where T : Object
            {
                if (null != obj)
                {
                    Object.Destroy(obj, t);
                }
            }
            public static void DestroyIf<T>(ref T obj) where T : Object
            {
                if (null != obj)
                {
                    Object.Destroy(obj);
                    obj = null;
                }
            }
            public static void DestroyIf<T>(ref T obj, float t) where T : Object
            {
                if (null != obj)
                {
                    Object.Destroy(obj, t);
                    obj = null;
                }
            }

#if UNITY_WSA
        public static void OnMainThread(ThreadTask task)
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(task, false);
        }

        public static void OnMainThreadAfter(float delay, ThreadTask task) {
            if (delay > 0.0f) {
                LeanTween.delayedCall (delay, () => {
                    OnMainThread (task);
                });
            } else {
                OnMainThread (task);
            }
        }
#endif

#if UNITY_EDITOR
            public static bool ObjectIsPrefabInstance(GameObject gameObject)
            {
                bool isPrefabInstance = PrefabUtility.GetPrefabParent(gameObject) != null && PrefabUtility.GetPrefabObject(gameObject.transform) != null;
                return isPrefabInstance;
            }
            public static bool ObjectIsPrefabOriginal(GameObject gameObject)
            {
                bool isPrefabOriginal = PrefabUtility.GetPrefabParent(gameObject) == null && PrefabUtility.GetPrefabObject(gameObject.transform) != null;
                return isPrefabOriginal;
            }
            public static bool ObjectIsDisconnectedPrefabInstance(GameObject gameObject)
            {
                bool isDisconnectedPrefabInstance = PrefabUtility.GetPrefabParent(gameObject) != null && PrefabUtility.GetPrefabObject(gameObject.transform) == null;
                return isDisconnectedPrefabInstance;
            }
#endif
        }
    }

#if (UNITY_METRO && !UNITY_EDITOR)
class LocateOurAssembly { };

namespace AppDomain
{
    public class CurrentDomain
    { 
        static Assembly[] allAssemblies = new Assembly[1] {
            typeof(LocateOurAssembly).GetTypeInfo().Assembly // I think for hololens we only get one assembly.
        };

        public static Assembly[] GetAssemblies()
        {
            return allAssemblies;
        }
    }
}

namespace System.Reflection
{
    [SerializableAttribute]
    [Runtime.InteropServices.ComVisibleAttribute(true)]
    public delegate bool TypeFilter(
        Type m,
        object filterCriteria
    );
}
#endif

#if (false && !UNITY_WSA)
namespace UnityEngine.VR.WSA
{
    public class HolographicSettings
    {
        public static void SetFocusPointForFrame(Vector3 pos, Vector3 dir, Vector3 velocity)
        {
        }
    }
}
#endif
}