using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolManager
{
    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>(); 

    private Transform root = null;

    public void Init()
    {
        if(root == null)
        {
            root = new GameObject("PoolRoot").transform;
            Object.DontDestroyOnLoad(root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.SetParent(root);

        pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (pools.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if(pools.ContainsKey(original.name) == false)
            CreatePool(original);

        return pools[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if(pools.ContainsKey(name) == false)
            return null;

        return pools[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in root)
            GameObject.Destroy(child.gameObject);

        pools.Clear();
    }
}
