using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] PlayerState state = PlayerState.Idle;
    public PlayerState State {
        get => state;
        set {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch(state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }
    
    private Vector3 destPos;
    private PlayerStat stat;

    private int mask = (int)DEFINE.Layer.Ground | (int)DEFINE.Layer.Monster;
    private GameObject lockTarget;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();

        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction += OnMouseEvent;

        // Managers.UI.ShowSceneUI<UI_Inventory>();
        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    private void Update()
    {
        switch(State)
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
            case PlayerState.Skill:
                UpdateSkill();
                break;
        }
    }

    private void UpdateDie()
    {
    }

    private void UpdateIdle()
    {
    }

    private void UpdateMoving()
    {
        if(lockTarget != null)
        {
            float distance = (destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }

        Vector3 dir = destPos - transform.position;

        if (dir.magnitude < 0.1f)
        {
            State = PlayerState.Idle;
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
                    State = PlayerState.Idle;
                return;
            }

            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", stat.MoveSpeed);
    }

    private void UpdateSkill()
    {
        if(lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion rotate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, 20 * Time.deltaTime);
        }
    }

    private void OnHitEvent()
    {
        if(lockTarget != null)
        {
            Stat targetStat = lockTarget.GetComponent<Stat>();
            int damage = Mathf.Max(0, stat.Attack - targetStat.Defense);

            targetStat.HP -= damage;
        }

        if(stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;

        }

    }

    private bool stopSkill = false;

    private void OnMouseEvent(DEFINE.MouseEvent evt)
    {
        if(State == PlayerState.Die)
            return;

        switch(State)
        {
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                if(evt == DEFINE.MouseEvent.PointerUp)
                {
                    stopSkill = true;
                }
                break;
        }
    }

    private void OnMouseEvent_IdleRun(DEFINE.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray, out hit, 100f, mask);
        // Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1f);
        
        switch(evt)
        {
            case DEFINE.MouseEvent.PointerDown:
                if(isHit)
                {
                    destPos = hit.point;
                    State = PlayerState.Moving;
                    stopSkill = false;

                    if (1 << hit.collider.gameObject.layer == (int)DEFINE.Layer.Monster)
                        lockTarget = hit.collider.gameObject;
                    else
                        lockTarget = null;
                }
                break;
            case DEFINE.MouseEvent.Press:
                if(lockTarget == null && isHit)
                    destPos = hit.point;
                break;
            case DEFINE.MouseEvent.PointerUp:
                stopSkill = true;
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

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill
    }
}
