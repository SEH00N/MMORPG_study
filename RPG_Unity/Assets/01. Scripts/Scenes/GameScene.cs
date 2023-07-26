using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = DEFINE.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inventory>();

        Dictionary<int, Data.Stat> dic = Managers.Data.Stats;
        
        gameObject.GetOrAddComponent<CursorController>();
    }

    public override void Clear()
    {

    }
}
