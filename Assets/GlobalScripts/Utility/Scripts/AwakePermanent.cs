namespace Utilities
{
    using UnityEngine;

    /// <summary>
    /// Sets the object dont destroy on load when it awakes
    /// </summary>
    public class AwakePermanent : MonoBehaviour
    {
        void Awake ()
        {
            DontDestroyOnLoad (gameObject);
        }
    }
}
