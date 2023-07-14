<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>


public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public event EventHandler<KeyValuePair<TKey, TValue>> Added;

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
        Added?.Invoke(this, new KeyValuePair<TKey, TValue>(key, value));
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public ICollection<TKey> Keys => _dictionary.Keys;

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public ICollection<TValue> Values => _dictionary.Values;

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    public int Count => _dictionary.Count;

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}