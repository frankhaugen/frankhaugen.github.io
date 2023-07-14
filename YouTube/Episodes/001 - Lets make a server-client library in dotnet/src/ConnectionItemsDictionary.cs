using System.Collections;

namespace Irc;

public class ConnectionItemsDictionary : IDictionary<object, object?>
{
    private readonly IDictionary<object, object?> _dictionary = new Dictionary<object, object?>();

    public object? this[object key] { get => _dictionary[key]; set => _dictionary[key] = value; }

    public ICollection<object> Keys => _dictionary.Keys;

    public ICollection<object?> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => _dictionary.IsReadOnly;

    public void Add(object key, object? value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<object, object?> item)
    {
        _dictionary.Add(item);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<object, object?> item)
    {
        return _dictionary.Contains(item);
    }

    public bool ContainsKey(object key)
    {
        return _dictionary.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<object, object?>[] array, int arrayIndex)
    {
        _dictionary.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<object, object?>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    public bool Remove(object key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<object, object?> item)
    {
        return _dictionary.Remove(item);
    }

    public bool TryGetValue(object key, out object? value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}