using UnityEngine;

public class UI_Inventory : UI_Scene
{
    public enum GameObjects
    {
        GridPanel,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        GameObject gridPanel = GetElement<GameObject>((int)GameObjects.GridPanel);
        foreach(Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        for(int i = 0; i < 12; i ++)
        {
            
            GameObject item = Managers.UI.MakeSubItem<UI_InventoryItem>(gridPanel.transform).gameObject;
            UI_InventoryItem invenItem = item.GetOrAddComponent<UI_InventoryItem>();
            invenItem.SetInfo($"집행검 {i}번");
        }
    }
}
