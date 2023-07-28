using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int order = 0;

    private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    private UI_Scene sceneUI = null;

    public GameObject Root { 
        get {
            GameObject root = GameObject.Find("UI_Root");
            if(root == null)
                root = new GameObject("UI_Root");

            return root;
        } 
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if(sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if(parent != null)
            go.transform.SetParent(parent);
        
        Canvas canvas= go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
            
        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if(parent != null)
            go.transform.SetParent(parent);
            
        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        this.sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if(popupStack.Count == 0)
            return;

        if(popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if(popupStack.Count == 0)
            return;

        UI_Popup popup = popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        order--;
    }

    public void CloseAllPopupUI()
    {
        while(popupStack.Count != 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        sceneUI = null;
    }
}