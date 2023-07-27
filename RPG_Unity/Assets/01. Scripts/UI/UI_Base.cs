using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;

public abstract class UI_Base : MonoBehaviour
{
    private Dictionary<Type, Object[]> objectDictionary = new Dictionary<Type, Object[]>();

    public abstract void Init();
    
    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);

        Object[] objects = new Object[names.Length];
        objectDictionary.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
            if(typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if(objects[i] == null)
                Debug.Log($"Failed to Bind ({names[i]})");
        }
    }

    protected T GetElement<T>(int idx) where T : Object
    {
        Object[] objects = null;
        if(objectDictionary.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetGameObject(int idx) => GetElement<GameObject>(idx);
    protected Text GetText(int idx) => GetElement<Text>(idx);
    protected Button GetButton(int idx) => GetElement<Button>(idx);
    protected Image GetImage(int idx) => GetElement<Image>(idx);

    public static void BindEvent(GameObject go, Action<PointerEventData> action, DEFINE.UIEvent type = DEFINE.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        
        switch(type)
        {
            case DEFINE.UIEvent.Click:
                evt.OnClickHandler += action;
                break;
            case DEFINE.UIEvent.Drag:
                evt.OnDragHandler += action;
                break;
            
        }
        evt.OnDragHandler += (PointerEventData data) => evt.transform.position = data.position;
    }
}
