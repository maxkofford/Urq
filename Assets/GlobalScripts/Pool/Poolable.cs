namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents a poolable prefab
    /// If its the prefab then isPrefab is set to true
    /// If its not then MyPrefab should be set to the prefab and isPrefab should be set to false
    /// Should only be instanced 
    /// </summary>
    public class Poolable : MonoBehaviour
    {
        private bool isPrefab = true;

        public Poolable MyPrefab
        {
            get;
            set;
        }

        public bool IsPrefab()
        {
            return isPrefab;
        }

        private void Awake()
        {
            if (!PoolManager.IsPoolSpawning())
            {
                Debug.LogError("This is supposed to be pooled and its not being pooled!");
            }

            isPrefab = false;
        }

        public void ReturnMe()
        {
            PoolManager.ReturnAPooled(this);
        }
    }
}