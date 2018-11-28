namespace Utilities
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;


    public class PreInitialize : MonoBehaviour
    {
        //[System.Serializable]
        //public class UnityPreInitializeEvent : UnityEvent<object> { };
        //public UnityPreInitializeEvent OnPreInitializationAwake;
        //public UnityPreInitializeEvent OnPreInitializationStart;

        public UnityEvent OnPreInitializationAwake;
        public UnityEvent OnPreInitializationStart;

        void Awake()
        {
            //Debug.Log("PreInitialize: Awake");
            //OnPreInitializationAwake.Invoke(this);
            OnPreInitializationAwake.Invoke();
        }

        void Start()
        {
            //Debug.Log("PreInitialize: Start");
            //OnPreInitializationStart.Invoke(this);
            OnPreInitializationStart.Invoke();
        }
    }
}
