using UnityEngine;

public class TCollision : MonoBehaviour
{
    // 1. 나한테 Rigidbody가 있어야 한다 (IsKinematic : off)
    // 2. 나한테 Collider가 있어야 한다 (IsTrigger : off)
    // 3. 상대방한테 Collider가 있어야 한다 (IsTrigger : off)
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
    }

    // 1. 둘 다 Collider가 있어야 한다
    // 2. 둘 중 하나는 IsTrigger : on
    // 3. 둘 중 하나는 Rigidbody가 있어야 한다
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }

    private void Update()
    {
        // Debug.DrawRay(transform.position + Vector3.up, transform.forward * 10, Color.red);

        // RaycastHit hit;
        // if(Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 10))
        //     Debug.Log($"RayCast! {hit.collider.name}");

        // RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, 10);
        // foreach(RaycastHit _hit in hits)
        //     Debug.Log($"RayCast! {_hit.collider.name}");

        // Local <-> World <-> Viewport <-> Screen (화면)
        // Debug.Log(Input.mousePosition); // Screen
        // Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); // Viewport

        // World

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Vector3 dir = mousePos - Camera.main.transform.position;
            // dir = dir.normalized;

            // Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1f);
            
            // RaycastHit hit;
            // if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100f))
            //     Debug.Log(hit.collider.name);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            // int mask = (1 << 6) | (1 << 7);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, mask))
                Debug.Log(hit.collider.name);

        }
    }
}