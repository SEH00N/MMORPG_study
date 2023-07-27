using System.Reflection.Emit;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] 
    protected int level;
    [SerializeField] 
    protected int hp;
    [SerializeField] 
    protected int maxHP;

    [SerializeField] 
    protected int attack;
    [SerializeField] 
    protected int defense;

    [SerializeField] 
    protected float moveSpeed;
    
    public int      Level   { get => level; set => level = value; }
    public int      HP      { get => hp; set => hp = value; }
    public int      MaxHP   { get => maxHP; set => maxHP = value; }
    public int      Attack  { get => attack; set => attack = value; }
    public int      Defense { get => defense; set => defense = value; }
    public float    MoveSpeed   { get => moveSpeed; set => moveSpeed = value; }

    private void Start()
    {
        level = 1;
        hp = 100;
        maxHP = 100;
        attack = 10;
        defense = 5;
        moveSpeed = 5f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.attack - Defense);
        HP -= damage;

        if(HP <= 0)
        {
            HP = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;
        if (playerStat != null)
        {
            playerStat.Exp += 150;
        }

        Managers.Game.Despawn(gameObject);
    }
}
