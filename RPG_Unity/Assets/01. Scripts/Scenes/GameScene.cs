using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = DEFINE.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Inventory>();
    }

    public override void Clear()
    {

    }
}
