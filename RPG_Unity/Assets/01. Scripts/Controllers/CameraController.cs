using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] DEFINE.CameraMode mode = DEFINE.CameraMode.QuaterView;
    [SerializeField] Vector3 delta = new Vector3(0.0f, 6.0f, -5.0f);
    [SerializeField] GameObject player = null;

    private void LateUpdate()
    {
        if(mode == DEFINE.CameraMode.QuaterView)
        {
            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
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
