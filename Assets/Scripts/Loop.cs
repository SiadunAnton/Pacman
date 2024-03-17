using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    private IEnumerable<CustomUpdatable> _updatables;
    private Dictionary<CustomUpdatable, float> _timeStorage;

    private void Awake()
    {
        _updatables = GetComponentsInChildren<CustomUpdatable>();
        _timeStorage = new Dictionary<CustomUpdatable, float>();
    }

    private void Start()
    {
        foreach (var updatable in _updatables)
            _timeStorage.Add(updatable,Time.time);
    }

    private void Update()
    {
        foreach (var updatable in _updatables)
            if (Time.time - _timeStorage[updatable] >= updatable.Speed && updatable.enabled)
            {
                updatable.CustomUpdate();
                _timeStorage[updatable] = Time.time;
            }
    }
}
