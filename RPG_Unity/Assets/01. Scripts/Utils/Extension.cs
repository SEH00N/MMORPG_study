using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, DEFINE.UIEvent type = DEFINE.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
}
