using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{   
    private PlayerState state = PlayerState.Idle;
    private CursorType cursorType = CursorType.None;
    private Vector3 destPos;

    private PlayerStat stat;

    private Texture2D attackIcon; 
    private Texture2D handIcon; 

    private void Start()
    {
        attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        stat = GetComponent<PlayerStat>();

        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction += OnMouseEvent;

        // Managers.UI.ShowSceneUI<UI_Inventory>();
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

        if (dir.magnitude < 0.01f)
        {
            state = PlayerState.Idle;
            return;
        }
        else
        {
            NavMeshAgent nav = GetComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0f, dir.magnitude);
            // nav.SetDestination(destPos);
            nav.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    state = PlayerState.Idle;
                return;
            }

            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", stat.MoveSpeed);
    }

    private void Update()
    {
        UpdateMouseCursor();

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

    private void UpdateMouseCursor()
    {
        if(Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            if(1 << hit.collider.gameObject.layer == (int)DEFINE.Layer.Monster)
            {
                if(cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackIcon, new Vector2(attackIcon.width / 5, 0), CursorMode.Auto);
                    cursorType = CursorType.Attack;
                }
            }
            else
            {
                if(cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(handIcon, new Vector2(handIcon.width / 3, 0), CursorMode.Auto);
                    cursorType = CursorType.Hand;
                }
            }
        }
    }

    private int mask = (int)DEFINE.Layer.Ground | (int)DEFINE.Layer.Monster;
    private GameObject lockTarget;

    private void OnMouseEvent(DEFINE.MouseEvent e)
    {
        if(state == PlayerState.Die)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray, out hit, 100f, mask);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        
        switch(e)
        {
            case DEFINE.MouseEvent.Press:
                if(lockTarget != null)
                    destPos = lockTarget.transform.position;
                else if(isHit)
                    destPos = hit.point;
                break;
            case DEFINE.MouseEvent.PointerDown:
                if(isHit)
                {
                    destPos = hit.point;
                    state = PlayerState.Moving;

                    if (1 << hit.collider.gameObject.layer == (int)DEFINE.Layer.Monster)
                        lockTarget = hit.collider.gameObject;
                    else
                        lockTarget = null;
                }
                break;
            case DEFINE.MouseEvent.PointerUp:
                lockTarget = null;
                break;
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
            transform.Translate(Vector3.forward * Time.deltaTime * stat.MoveSpeed, Space.World);
            // transform.Translate(Vector3.forward * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 10f);
            transform.Translate(Vector3.back * Time.deltaTime * stat.MoveSpeed, Space.World);
            // transform.Translate(Vector3.back * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.back) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
            transform.Translate(Vector3.left * Time.deltaTime * stat.MoveSpeed, Space.World);
            // transform.Translate(Vector3.left * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 10f);
            transform.Translate(Vector3.right * Time.deltaTime * stat.MoveSpeed, Space.World);
            // transform.Translate(Vector3.right * Time.deltaTime * speed);
            // transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * speed;
        }

        // state = PlayerState.Idle;
    }

    public enum CursorType
    {
        None, 
        Attack,
        Hand
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill
    }
}
