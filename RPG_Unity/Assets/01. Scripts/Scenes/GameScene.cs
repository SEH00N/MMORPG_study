using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = DEFINE.Scene.Game;
        //Managers.UI.ShowSceneUI<UI_Inventory>();
        
        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(DEFINE.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        GameObject go = new GameObject("SpawningPool");
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {

    }
}
