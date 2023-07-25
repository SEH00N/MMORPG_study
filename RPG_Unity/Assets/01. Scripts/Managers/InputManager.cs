using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public event Action KeyAction = null;
    public event Action<DEFINE.MouseEvent> MouseAction = null;

    private bool pressed = false;

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }

    public void OnUpdate()
    {
        // if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if(Input.anyKey)
            KeyAction?.Invoke();

        if(Input.GetMouseButton(0))
        {
            MouseAction?.Invoke(DEFINE.MouseEvent.Press);
            pressed = true;
        }
        else
        {
            if(pressed)
                MouseAction?.Invoke(DEFINE.MouseEvent.Click);
            pressed = false;
        }
    }
}
