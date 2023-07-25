using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public DEFINE.Scene SceneType { get; protected set; } = DEFINE.Scene.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
    
        if(obj == null)
            Managers.Resource.Instantiate("UI/EventSystem");
    }   

    public abstract void Clear(); 
}
