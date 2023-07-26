using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] int exp;
    [SerializeField] int gold;

    public int Exp { get => exp; set => exp = value; }
    public int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        level = 1;
        hp = 100;
        maxHP = 100;
        attack = 10;
        defense = 5;
        moveSpeed = 10f;
    }
}
