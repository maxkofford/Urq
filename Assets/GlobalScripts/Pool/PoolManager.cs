namespace Urq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// For pooling stuff - pools prefabs and instantiates them 
    /// </summary>
    public static class PoolManager 
    {
        private const int defaultPoolAmount = 5;

        private static bool isPoolSpawned = false;

        private static Dictionary<Poolable, Queue<Poolable>> prefabToPooled = new Dictionary<Poolable, Queue<Poolable>>();       

        private static GameObject poolHost;

        public static bool IsPoolSpawning()
        {
            return isPoolSpawned;
        }

        /// <summary>
        /// Adds a amount of instances of a prefab to the prefab pool
        /// </summary>
        public static void AddToPool(Poolable prefab, int amount)
        {
            if (poolHost == null)
            {
                poolHost = new GameObject("PoolHost");
            }

            Queue<Poolable> pooledStuff;
            if (!prefabToPooled.TryGetValue(prefab, out pooledStuff))
            {
                pooledStuff = new Queue<Poolable>();
                prefabToPooled.Add(prefab, pooledStuff);
            }

            for (int i = 0; i < amount; i++)
            {
                isPoolSpawned = true;
                Poolable currentNewPoolable = GameObject.Instantiate(prefab, poolHost.transform);
                isPoolSpawned = false;

                currentNewPoolable.MyPrefab = prefab;
                currentNewPoolable.gameObject.SetActive(false);
                pooledStuff.Enqueue(currentNewPoolable);
            }
        }
       
        /// <summary>
        /// Gets a object out of the object pool
        /// </summary>
        public static Poolable GetAPooled(Poolable prefab)
        {
            if (!prefab.IsPrefab())
            {
                Debug.LogError("Trying to get a pooled object for a non prefab!");
            }

            Queue<Poolable> pooledStuff;
            if (!prefabToPooled.TryGetValue(prefab, out pooledStuff))
            {
                AddToPool(prefab, defaultPoolAmount);
                return GetAPooled(prefab);
            }
            else
            {
                if (pooledStuff.Count < 1)
                {
                    AddToPool(prefab, defaultPoolAmount);
                }

                Poolable usedPoolable = pooledStuff.Dequeue();
                usedPoolable.gameObject.SetActive(true);
                return usedPoolable;
            }
        }

        /// <summary>
        /// Returns a object to the prefab pool
        /// </summary>
        public static void ReturnAPooled(Poolable aPooled)
        {
            if (aPooled.IsPrefab())
            {
                Debug.LogError("Trying to return a prefab object to a pool!");
                return;
            }

            Poolable prefab = aPooled.MyPrefab;
            if(prefab != null)
            {
                Queue<Poolable> pooledStuff;
                if (prefabToPooled.TryGetValue(prefab, out pooledStuff))
                {
                    pooledStuff.Enqueue(aPooled);
                    aPooled.gameObject.SetActive(false);
                    aPooled.transform.SetParent(poolHost.transform);
                    return;
                }
                else
                {
                    Debug.LogError("Somehow the pooled stuff for this prefab doesnt exist?");
                    return;
                }
            }
            else
            {
                Debug.LogError("The item thats trying to be returned does not have is prefab set!");
                return;
            }
        }
    }
}