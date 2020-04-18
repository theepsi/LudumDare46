using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    public List<PooledObject> PooledObjects;

    private Dictionary<string, PooledObject> pools;

    [Serializable]
    public struct PooledObject
    {
        public GameObject objectToPool;
        public List<GameObject> Pool;
        public int PooledAmount;

        public void ObjectPooler()
        {
            Pool = new List<GameObject>();
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        // Initialize pools
        pools = new Dictionary<string, PooledObject>();
        int count = PooledObjects.Count;
        for (int i = 0; i < count; ++i)
        {
            for (int j = 0; j < PooledObjects[i].PooledAmount; ++j)
            {
                GameObject go = Instantiate(PooledObjects[i].objectToPool, transform);
                go.SetActive(false);
                PooledObjects[i].Pool.Add(go);
            }
            pools.Add(PooledObjects[i].objectToPool.name, PooledObjects[i]);
        }
    }

    public GameObject GetPooledObject(string type, Transform parent = null)
    {
        for (int i = 0; i < pools[type].Pool.Count; ++i)
        {
            if (!pools[type].Pool[i].activeInHierarchy)
            {
                GameObject obj = pools[type].Pool[i];
                if (parent != null)
                {
                    obj.transform.SetParent(parent, false);
                }
                return pools[type].Pool[i];
            }
        }

        GameObject go = Instantiate(pools[type].objectToPool, transform);

        go.SetActive(false);

        if (parent != null)
        {
            go.transform.SetParent(parent, false);
        }

        pools[type].Pool.Add(go);
        return go;
    }
}