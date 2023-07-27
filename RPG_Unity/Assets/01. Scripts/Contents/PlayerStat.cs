using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] int exp;
    [SerializeField] int gold;

    public int Exp { 
        get => exp; 
        set {
            exp = value;
            
            int level = Level;
            while(true)
            {
                Data.Stat stat;
                if(Managers.Data.Stats.TryGetValue(level + 1, out stat) == false)
                    break;

                if(exp < stat.totalExp)
                    break;

                level++;
            }

            if(level != Level)
            {
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        level = 1;

        // Dictionary<int, Data.Stat> stats = Managers.Data.Stats;
        exp = 0;
        defense = 5;
        moveSpeed = 10f;
        gold = 0;

        SetStat(level);
    }

    public void SetStat(int level)
    {
        Data.Stat stat = Managers.Data.Stats[level];

        hp = stat.maxHP;
        maxHP = stat.maxHP;
        attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
    }
}
