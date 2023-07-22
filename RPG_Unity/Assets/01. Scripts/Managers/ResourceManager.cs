using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object => Resources.Load<T>(path);
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            Debug.Log($"Failed to Load Prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject obj)
    {
        if(obj == null)
            return;

        Destroy(obj);
    }
}
