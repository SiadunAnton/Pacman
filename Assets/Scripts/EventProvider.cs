using System.Collections.Generic;
using System;

public class EventProvider
{
    private Dictionary<string, Action> _storage;

    public EventProvider()
    {
        _storage = new Dictionary<string, Action>();
    }

    public void Register(string eventName)
    {
        _storage.Add(eventName, null);
    }

    public void Subscribe(string eventName, Action action)
    {
        _storage[eventName] += action;
    }

    public void Invoke(string eventName)
    {
        _storage[eventName]?.Invoke();
    }
}
