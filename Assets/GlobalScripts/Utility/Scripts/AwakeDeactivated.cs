namespace Utilities
{
    using UnityEngine;

    /// <summary>
    /// Deactivates on awake
    /// </summary>
    public class AwakeDeactivated : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
