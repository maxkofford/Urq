namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Poolable))]
    public class PoolableEditor : Editor
    {
        List<Poolable> myTargets;

        void OnEnable()
        {
            myTargets = new List<Poolable>();
            foreach (var o in targets)
            {
                Poolable p = o as Poolable;
                if (p != null)
                {
                    myTargets.Add(p);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Return Me To Pool"))
                {
                    foreach (Poolable myTarget in myTargets)
                    {
                        myTarget.ReturnMe();
                    }
                }


                if (GUILayout.Button("Check prefab from pool (only use on prefab)"))
                {
                    foreach (Poolable myTarget in myTargets)
                    {
                        PoolManager.GetAPooled(myTarget);
                    }
                }

            }
        }
    }
}