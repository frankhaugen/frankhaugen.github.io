<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

public class ConcurrentObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
    private readonly ConcurrentQueue<KeyValuePair<TKey, TValue>> _addedQueue = new ConcurrentQueue<KeyValuePair<TKey, TValue>>();
    private readonly object _eventLock = new object();
    private bool _raisingEvent = false;

    public event EventHandler<KeyValuePair<TKey, TValue>> Added;

    public void Add(TKey key, TValue value)
    {
        _lock.EnterWriteLock();
        try
        {
            _dictionary.Add(key, value);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
        _addedQueue.Enqueue(new KeyValuePair<TKey, TValue>(key, value));
        RaiseAddedEvent();
    }

    private void RaiseAddedEvent()
    {
        lock (_eventLock)
        {
            if (_raisingEvent)
            {
                return;
            }
            _raisingEvent = true;
        }
        Task.Run(() =>
        {
            while (_addedQueue.TryDequeue(out var item))
            {
                Added?.Invoke(this, item);
            }
            lock (_eventLock)
            {
                _raisingEvent = false;
            }
        });
    }

    public bool ContainsKey(TKey key)
    {
        _lock.EnterReadLock();
        try
        {
            return _dictionary.ContainsKey(key);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _dictionary.Keys;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public bool Remove(TKey key)
    {
        _lock.EnterWriteLock();
        try
        {
            return _dictionary.Remove(key);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        _lock.EnterReadLock();
        try
        {
            return _dictionary.TryGetValue(key, out value);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _dictionary.Values;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _dictionary[key];
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        set
        {
            _lock.EnterWriteLock();
            try
            {
                _dictionary[key] = value;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _dictionary.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        _lock.EnterReadLock();
        try
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _lock.EnterReadLock();
        try
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public int Count
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _dictionary.Count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        _lock.EnterWriteLock();
        try
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        _lock.EnterReadLock();
        try
        {
            return _dictionary.GetEnumerator();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}