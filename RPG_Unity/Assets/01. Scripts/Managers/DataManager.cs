using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private Dictionary<int, Stat> stats = new Dictionary<int, Stat>();
    public Dictionary<int, Stat> Stats => stats;
    private StatData data;

    public void Init()
    {
        stats = LoadJson<StatData, int, Stat>("StatData").MakeDictionary();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/StatData");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
