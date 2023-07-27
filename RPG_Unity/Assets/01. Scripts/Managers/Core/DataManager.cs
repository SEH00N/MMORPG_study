using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private Dictionary<int, Data.Stat> stats = new Dictionary<int, Data.Stat>();
    public Dictionary<int, Data.Stat> Stats => stats;
    private Data.StatData data;

    public void Init()
    {
        stats = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDictionary();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/StatData");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
