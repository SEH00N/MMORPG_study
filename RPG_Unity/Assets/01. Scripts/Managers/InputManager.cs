using System;
using UnityEngine;

public class InputManager
{
    public event Action KeyAction = null;
    public event Action<DEFINE.MouseEvent> MouseAction = null;

    private bool pressed = false;

    public void OnUpdate()
    {
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
