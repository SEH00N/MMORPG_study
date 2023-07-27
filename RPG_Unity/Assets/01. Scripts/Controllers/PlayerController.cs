using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{   
    private int mask = (int)DEFINE.Layer.Ground | (int)DEFINE.Layer.Monster;
    private bool stopSkill = false;

    private PlayerStat stat;

    public override void Init()
    {
        WorldObjectType = DEFINE.WorldObject.Player;
        stat = GetComponent<PlayerStat>();

        Managers.Input.MouseAction += OnMouseEvent;

        // Managers.UI.ShowSceneUI<UI_Inventory>();
        if(GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        if(lockTarget != null)
        {
            float distance = (destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = DEFINE.State.Skill;
                return;
            }
        }

        Vector3 dir = destPos - transform.position;
        dir.y = 0;

        if (dir.magnitude < 0.1f)
        {
            State = DEFINE.State.Idle;
            return;
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.red);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = DEFINE.State.Idle;
                return;
            }
            
            float moveDist = Mathf.Clamp(stat.MoveSpeed * Time.deltaTime, 0f, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            // transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }
    }

    protected override void UpdateSkill()
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
            targetStat.OnAttacked(stat);
        }

        if(stopSkill)
        {
            State = DEFINE.State.Idle;
        }
        else
        {
            State = DEFINE.State.Skill;

        }

    }
    
    private void OnMouseEvent(DEFINE.MouseEvent evt)
    {
        if(State == DEFINE.State.Die)
            return;

        switch(State)
        {
            case DEFINE.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case DEFINE.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case DEFINE.State.Skill:
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
                    State = DEFINE.State.Moving;
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
}








//     private void OnKeyboard()
//     {
//         // Local -> World
//         // TransformDirection

//         // World -> Local
//         // InverseTransformDirection

//         // transform.rotation
//         // transform.eulerAngles
//         // transform.Rotate()

//         if(Input.GetKey(KeyCode.W))
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * 10f);
//             transform.Translate(Vector3.forward * Time.deltaTime * stat.MoveSpeed, Space.World);
//             // transform.Translate(Vector3.forward * Time.deltaTime * speed);
//             // transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
//         }
//         if(Input.GetKey(KeyCode.S))
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), Time.deltaTime * 10f);
//             transform.Translate(Vector3.back * Time.deltaTime * stat.MoveSpeed, Space.World);
//             // transform.Translate(Vector3.back * Time.deltaTime * speed);
//             // transform.position += transform.TransformDirection(Vector3.back) * Time.deltaTime * speed;
//         }
//         if(Input.GetKey(KeyCode.A))
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), Time.deltaTime * 10f);
//             transform.Translate(Vector3.left * Time.deltaTime * stat.MoveSpeed, Space.World);
//             // transform.Translate(Vector3.left * Time.deltaTime * speed);
//             // transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * speed;
//         }
//         if(Input.GetKey(KeyCode.D))
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 10f);
//             transform.Translate(Vector3.right * Time.deltaTime * stat.MoveSpeed, Space.World);
//             // transform.Translate(Vector3.right * Time.deltaTime * speed);
//             // transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * speed;
//         }

//         // state = PlayerState.Idle;
//     }