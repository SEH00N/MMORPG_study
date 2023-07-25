using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = DEFINE.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inventory>();

        Dictionary<int, Stat> dic = Managers.Data.Stats;
    }

    public override void Clear()
    {

    }
}
