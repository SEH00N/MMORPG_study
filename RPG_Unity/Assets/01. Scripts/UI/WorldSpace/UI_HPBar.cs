using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    private Stat stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = stat.HP / (float)stat.MaxHP;
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        GetGameObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
