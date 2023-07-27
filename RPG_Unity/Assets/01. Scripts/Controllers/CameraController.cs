using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] DEFINE.CameraMode mode = DEFINE.CameraMode.QuaterView;
    [SerializeField] Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f);
    [SerializeField] GameObject player = null;

    public void SetPlayer(GameObject player) => this.player = player;

    private void LateUpdate()
    {
        if(mode == DEFINE.CameraMode.QuaterView)
        {
            if(player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, (int)DEFINE.Layer.Block))
            {
                float distance = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * distance;
            }
            else
            {
                transform.position = player.transform.position + delta;
                transform.LookAt(player.transform);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        mode = DEFINE.CameraMode.QuaterView;
        this.delta = delta;
    }
}
