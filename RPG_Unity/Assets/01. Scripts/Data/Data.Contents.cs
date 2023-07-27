using System;
using System.Collections.Generic;

namespace Data
{
    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHP;
        public int attack;
        public int totalExp;

        public override string ToString()
        {
            return $"level : {level}, maxHP : {maxHP}, attack : {attack}, totalExp : {totalExp}";
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
}
