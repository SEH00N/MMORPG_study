using System.Collections.Generic;

public interface ILoader<Key, Value>
{
    public Dictionary<Key, Value> MakeDictionary();
}