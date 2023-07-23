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

        GameObject go = Object.Instantiate(prefab, parent);
        int index = go.name.IndexOf("(Clone)");
        if(index > 0)
            go.name = go.name.Substring(0, index);

        return go;
    }

    public void Destroy(GameObject obj)
    {
        if(obj == null)
            return;

        GameObject.Destroy(obj);
    }
}
