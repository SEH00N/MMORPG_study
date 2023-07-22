using UnityEngine;

public class TPrefab : MonoBehaviour
{
    private void Start()
    {
        GameObject tank = Managers.Resource.Instantiate("Tank");
        Destroy(tank, 3.0f);
    }
}
