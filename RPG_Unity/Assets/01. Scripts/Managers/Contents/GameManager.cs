using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject player;
    private HashSet<GameObject> monsters = new HashSet<GameObject>();

    public event Action<int> OnSpawnEvent;

    public GameObject GetPlayer() => player;

    public GameObject Spawn(DEFINE.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case DEFINE.WorldObject.Monster:
                monsters.Add(go);
                OnSpawnEvent?.Invoke(1);
                break;
            case DEFINE.WorldObject.Player:
                player = go;
                break;
        }

        return go;
    }

    public DEFINE.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if(bc == null)
            return DEFINE.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        DEFINE.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case DEFINE.WorldObject.Monster:
            {
                if(monsters.Contains(go))
                {
                    monsters.Remove(go);
                    OnSpawnEvent?.Invoke(-1);
                }
            }
                break;
            case DEFINE.WorldObject.Player:
                if(player == go)
                    player = null;
                break;
        }

        Managers.Resource.Destroy(go);
    }
}