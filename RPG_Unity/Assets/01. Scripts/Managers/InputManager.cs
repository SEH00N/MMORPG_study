using System;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;

    public void OnUpdate()
    {
        if(Input.anyKey == false)
            return;

        KeyAction?.Invoke();
    }
}