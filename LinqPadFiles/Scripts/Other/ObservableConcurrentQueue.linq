<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>


using System;
using System.Collections.Concurrent;

public class ObservableConcurrentQueue<T> : IDisposable
{
    private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
    public event EventHandler<T> ItemEnqueued;

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        ItemEnqueued?.Invoke(this, item);
    }

    public bool TryDequeue(out T result)
    {
        return _queue.TryDequeue(out result);
    }

    public void Dispose()
    {
        ItemEnqueued = null;
    }
}