using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Button : UI_Popup
{
    private Dictionary<Type, Object[]> objectDictionary = new Dictionary<Type, Object[]>();

    public enum Buttons
    {
        PointButton,
    }

    public enum Texts
    {
        PointText,
        ScoreText,
    }

    public enum GameObjects
    {
        TestObject,
    }

    public enum Images
    {
        ItemIcon,
    }
    
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects)); 
        Bind<Image>(typeof(Images));

        GetElement<Text>((int)Texts.ScoreText).text = "Bind Test";

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (data) => go.transform.position = data.position, DEFINE.UIEvent.Drag);
    }

    public void OnButtonClicked()
    {
        // score++;
    }
}
