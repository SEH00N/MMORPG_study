using System;
using System.Collections.Generic;

#region Stat
[Serializable]
public class Stat
{
    public int level;
    public int hp;
    public int attack;

    public override string ToString()
    {
        return $"level : {level}, hp : {hp}, attack : {attack}";
    }
}

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();

    public Dictionary<int, Stat> MakeDictionary()
    {
        Dictionary<int, Stat> dic = new Dictionary<int, Stat>();
        stats.ForEach(stat => dic.Add(stat.level, stat));

        return dic;
    }
}
#endregion