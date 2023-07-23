using UnityEngine;
using Object = UnityEngine.Object;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if(component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject root, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(root, name, recursive);
        if(transform == null)
            return null;
        
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject root, string name = null, bool recursive = false) where T : Object
    {
        if(root == null)
            return null;


        if(recursive == false)
        {
            for(int i = 0; i < root.transform.childCount; i++)
            {
                Transform transform = root.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in root.GetComponentsInChildren<T>())
                if(string.IsNullOrEmpty(name) || component.name == name)
                    return component;
        }

        return null;
    }
}
