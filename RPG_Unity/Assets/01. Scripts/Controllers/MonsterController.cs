using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    [SerializeField] float scanRange = 10f;
    [SerializeField] float attackRange = 2f;

    private Stat stat;

    public override void Init()
    {
        WorldObjectType = DEFINE.WorldObject.Monster;
        stat = GetComponent<Stat>();

        if(GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if(player == null)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if(distance <= scanRange)
        {
            lockTarget = player;
            State = DEFINE.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        if(lockTarget != null)
        {
            destPos = lockTarget.transform.position;
            float distance = (destPos - transform.position).magnitude;
            if(distance <= attackRange)
            {
                NavMeshAgent nav = GetComponent<NavMeshAgent>();
                nav.SetDestination(transform.position);
                State = DEFINE.State.Skill;
                return;
            }
        }

        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = DEFINE.State.Idle;
            return;
        }
        else
        {
            NavMeshAgent nav = GetComponent<NavMeshAgent>();
            nav.SetDestination(destPos);
            nav.speed = stat.MoveSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }
    }

    protected override void UpdateSkill()
    {
        if (lockTarget != null)
        {
            Vector3 dir = lockTarget.transform.position - transform.position;
            Quaternion rotate = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, 20 * Time.deltaTime);
        }
    }

    private void OnHitEvent()
    {
        if (lockTarget != null)
        {
            Stat targetStat = lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(stat);

            if(targetStat.HP > 0)
            {
                float distance = (lockTarget.transform.position - transform.position).magnitude;
                if(distance <= attackRange)
                    State = DEFINE.State.Skill;
                else
                    State = DEFINE.State.Moving;
            }
            else
            {
                State = DEFINE.State.Idle;
            }
        }
        else
            State = DEFINE.State.Idle;
    }
}
