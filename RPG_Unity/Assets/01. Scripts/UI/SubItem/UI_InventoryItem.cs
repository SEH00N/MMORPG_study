using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryItem : UI_Base
{
    public enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    private string itemName;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        GetElement<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = itemName;

        GetElement<GameObject>((int)GameObjects.ItemIcon).AddUIEvent(evt => Debug.Log($"아이템 클릭! : {itemName}"));
    }

    public void SetInfo(string itemName)
    {
        this.itemName = itemName;
    }
}
