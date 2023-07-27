using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject Original { get; private set; }
    public Transform Root { get; set; }

    private Stack<Poolable> poolStack = new Stack<Poolable>();

    public void Init(GameObject original, int count = 5)
    {
        Original = original;
        Root = new GameObject($"{original.name}Root").transform;
        
        for(int i = 0; i < count; i++)
            Push(Create());
    }

    private Poolable Create()
    {
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = go.name.Replace("(Clone)", "");
        return go.GetOrAddComponent<Poolable>();
    }

    public void Push(Poolable poolable)
    {
        if(poolable == null)
            return;

        poolable.gameObject.SetActive(false);
        poolable.transform.SetParent(Root);
        poolable.IsUsing = false;

        poolStack.Push(poolable);
    }

    public Poolable Pop(Transform parent)
    {
        Poolable poolable;

        if(poolStack.Count > 0)
            poolable = poolStack.Pop();
        else
            poolable = Create();

        if(parent == null)
            poolable.transform.SetParent(Managers.Scene.CurrentScene.transform);

        poolable.transform.SetParent(parent);
        poolable.gameObject.SetActive(true);
        poolable.IsUsing = true;

        return poolable;
    }
}
