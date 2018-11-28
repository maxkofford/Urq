// Just a note on the use of DontDestroyOnLoad in the following implementation:
//
// From https://docs.unity3d.com/Manual/MultiSceneEditing.html:
//   It is recommended to avoid using DontDestroyOnLoad to persist manager GameObjects that you want to survive across scene loads.
//   Instead, create a manager scene that has all your managers and use SceneManager.LoadScene(<path>, LoadSceneMode.Additive)
//   and SceneManager.UnloadScene to manage your game progress.
//
// The current Unity implementation of DontDestroyOnLoad seems to be effectively doing what this recommendation says.
// It's conceivable that they'll deprecate that function before too long.

namespace Utilities
{
    using UnityEngine;

    internal static class SingletonShared
    {
        internal static bool ApplicationIsQuitting = false;
    }

    /// <summary>
    /// This singleton implementation is appropriate for classes that *DON'T* derive from MonoBehavior,
    /// and therefore aren't associated with a game object.
    /// </summary>
    /// <typeparam name="T">Pass the class deriving from this generic base class</typeparam>
    public class Singleton<T> : System.Object
        where T : Singleton<T>, new()
    {
        private static T g_instance;
        private static object g_lock = new object();

        public static T Instance
        {
            get { return g_instance ?? GetInstance(); } // Even if quitting we'll still return an existing singleton here. Is that good?
        }

        public static T GetInstance()
        {
            if (SingletonShared.ApplicationIsQuitting)
            {
                Debug.LogWarningFormat("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning default (null).",
                    typeof(T).Name);
                return default(T);
            }

            lock (g_lock)
            {
                if (null == g_instance)
                {
                    var alloc = new T();
                    g_instance = alloc;
                }
            }

            return g_instance;
        }

        ~Singleton()
        {
            SingletonShared.ApplicationIsQuitting = true;
            // Any reason to lock here?
            if (null != g_instance)
            {
                Debug.LogFormat("[Singleton] Instance '{0}' destroyed", typeof(T).Name);
                if (g_instance == this)
                {
                    g_instance = null;
                }
            }
        }
    }

    
    /// <summary>
    /// This singleton class is appropriate for classes derived from MonoBehaviour,
    /// which may be attached as components on a game object in a loaded scene or
    /// which require the runtime creation of a host game object.
    /// Try not to mix the two usages or you'll probably have trouble.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoSingleton<T>
    {
        private static T g_instance;
        private static object g_lock = new object();
        private static string g_defaultName;

        public static T Instance
        {
            get { return g_instance ?? GetInstance(); } // Even if quitting we'll still return an existing singleton here. Is that good?
        }

        public static T GetInstance()
        {
            if (SingletonShared.ApplicationIsQuitting)
            {
                Debug.LogWarningFormat("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning default (null).",
                    typeof(T).Name);
                return default(T);
            }

            lock (g_lock)
            {
                if (null == g_instance)
                {
                    var gameObject = new GameObject(DefaultName(), typeof(T));
                    Debug.Assert(g_instance != null);
                    DontDestroyOnLoad(gameObject);
                }
            }

            return g_instance;
        }

        public static string DefaultName()
        {
            return g_defaultName ?? (g_defaultName = string.Format("Singleton<{0}>", typeof(T).Name));
        }

        protected virtual void Awake()
        {
            // lock (g_lock) is equivalent to Monitor.Enter(g_lock)
            if (System.Threading.Monitor.TryEnter(g_lock)) // We might be allocating from within GetInstance()
            {
                try
                {
                    if (null == g_instance)
                    {
                        g_instance = this as T;
                        if (this.gameObject.transform != this.gameObject.transform.root)
                        {
                            Debug.LogFormat("[Singleton] Instance '{0}' moving this singleton object to the scene root so it can persist. ({1})\n"
                                + " Note that this means every component on the game object and every child object is going along with it!",
                                typeof(T).Name, this.gameObject.name);
                            this.gameObject.transform.parent = null;
                        }
                        DontDestroyOnLoad(this.gameObject);
                    }
                    else
                    {
                        Debug.LogFormat("[Singleton] Instance '{0}' unloading a duplicate singleton game object! ({1})\n"
                                + " Note that this means every component on the game object and every child object is being deleted along with it!",
                            typeof(T).Name, this.gameObject.name);
                        Destroy(this.gameObject);
                    }
                }
                finally
                {
                    System.Threading.Monitor.Exit(g_lock);
                }
            }
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// We set ApplicationIsQuitting to be sure we're not creating that buggy ghost object.
        /// </summary>
        protected virtual void OnDestroy()
        {
            SingletonShared.ApplicationIsQuitting = true;
            // Any reason to lock here?
            if (null != g_instance)
            {
                Debug.LogFormat("[Singleton] Instance '{0}' destroyed", typeof(T).Name);
                if (g_instance == this)
                {
                    g_instance = null;
                }
            }
        }
    }
}
