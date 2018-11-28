using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

using System.Collections;

namespace Utilities
{
    public class BakeFunction
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnPostprocessScene()
        {
            foreach (MonoBehaviour mb in UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour)))
            {
                IBakeable ib = mb as IBakeable;
                if (ib != null)
                    ib.Bake();
            }
        }

        public static bool PlayingInTheEditor()
        {
            return (Application.isPlaying && Application.isEditor);
        }
#endif
    }

    // Implement this interface in any component to get called at bake time
    // during a build or when you hit the "run" button in the editor.
    public interface IBakeable
    {
        void Bake();
    }
}
