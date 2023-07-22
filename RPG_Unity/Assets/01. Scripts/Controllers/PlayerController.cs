using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] float speed = 10f;

    private Vector3 destPos;

    private void Start()
    {
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private float wait_run_ratio = 0f;
    private PlayerState state = PlayerState.Idle;

    public enum PlayerState
    {
        Die,
        Moving,
        Idle
    }

    private void UpdateDie()
    {

    }

    private void UpdateIdle()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", 0);
    }

    private void UpdateMoving()
    {
        Vector3 dir = destPos - transform.position;

        if (dir.magnitude < 0.0001f)
            state = PlayerState.Idle;
        else
        {
            float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0f, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", speed);
    }

    private void Update()
    {
        switch(state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }
    }

    private void OnMouseClicked(DEFINE.MouseEvent e)
    {
        if(state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Wall")))
        {
            destPos = hit.point;
            state = PlayerState.Moving;
        }

    }    

    private void OnKeyboard()
    {
        // Local -> World
        // TransformDirection

        // World -> Local
        // InverseTransformDirection
        
        // transform.rotation
        // transform.eulerAngles
        // transform.Rotate()

        if(Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 10f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 10f);
            transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.back * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.back) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.left * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 10f);
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
            // transform.Translate(Vector3.right * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * speed;
        }

        state = PlayerState.Idle;
    }
}
