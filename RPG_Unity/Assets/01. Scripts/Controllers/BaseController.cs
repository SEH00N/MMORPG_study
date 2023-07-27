using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] protected DEFINE.State state = DEFINE.State.Idle;
    public DEFINE.State State {
        get => state;
        set {
            state = value;

            Animator anim = GetComponent<Animator>();
            switch(state)
            {
                case DEFINE.State.Die:
                    break;
                case DEFINE.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case DEFINE.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case DEFINE.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }
    
    [SerializeField] protected Vector3 destPos;
    [SerializeField] protected GameObject lockTarget;
    public DEFINE.WorldObject WorldObjectType { get; protected set; } = DEFINE.WorldObject.Unknown;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch(State)
        {
            case DEFINE.State.Die:
                UpdateDie();
                break;
            case DEFINE.State.Moving:
                UpdateMoving();
                break;
            case DEFINE.State.Idle:
                UpdateIdle();
                break;
            case DEFINE.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();

    protected virtual void UpdateDie() {}
    protected virtual void UpdateMoving() {}
    protected virtual void UpdateIdle() {}
    protected virtual void UpdateSkill() {}
}
