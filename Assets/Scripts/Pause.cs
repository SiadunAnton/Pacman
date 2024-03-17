using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pause : MonoBehaviour
{
    [SerializeField] private List<CustomUpdatable> _stoppingObjects;
    [SerializeField] private float _duration;

    private List<Animator> _stoppingAnimators;
    private Dictionary<CustomUpdatable, float> _savedSpeeds;
    private EventProvider _provider;
    private bool _isObjectsStopped = false;

    [Inject]
    public void Initialize(EventProvider provider)
    {
        _provider = provider;
    }

    private void Awake()
    {
        _savedSpeeds = new Dictionary<CustomUpdatable, float>();
        _stoppingAnimators = new List<Animator>();
        foreach (var stopping in _stoppingObjects)
        {
            _savedSpeeds.Add(stopping, stopping.Speed);
            _stoppingAnimators.Add(stopping.GetComponent<Animator>());
        }

        _provider.Register("OnPauseEnd"); 
    }

    private void Start()
    {
        _provider.Subscribe("OnPelletsAreOut", () => Run(_duration));
    }

    public void Run(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StoppingProcess(duration));
    }

    IEnumerator StoppingProcess(float duration)
    {
        if(!_isObjectsStopped)
        {
            foreach (var stopping in _stoppingObjects)
            {
                _savedSpeeds[stopping] = stopping.Speed;
                stopping.Speed = duration;
            }

            foreach (var animator in _stoppingAnimators)
            {
                animator.enabled = false;
            }
        }

        _isObjectsStopped = true;

        yield return new WaitForSeconds(duration);

        foreach (var stopping in _stoppingObjects)
        {
            stopping.Speed = _savedSpeeds[stopping];
        }

        foreach (var animator in _stoppingAnimators)
        {
            animator.enabled = true;
        }
        _isObjectsStopped = false;
        _provider.Invoke("OnPauseEnd");
    }
}
