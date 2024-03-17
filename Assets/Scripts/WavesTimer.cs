using System.Collections;
using UnityEngine;
using Zenject;

public class WavesTimer : MonoBehaviour
{
    private float[] _wavesDuration = { 7f, 20f, 7f, 20f, 5f, 20f, 5f, float.MaxValue };
    private int _currentWaveIndex = 0;

    private EventProvider _provider;

    [Inject]
    public void Initialize(EventProvider provider)
    {
        _provider = provider;
    }

    private void Awake()
    {
        _provider.Register("OnWaveStart");
        _provider.Register("OnWaveEnd");
    }

    private void Start()
    {
        StartCoroutine(ResetWave());

        _provider.Subscribe("OnAmplificationStart", Stop);
        _provider.Subscribe("OnAmplificationHasEnded", Reset);
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void Reset()
    {
        StartCoroutine(ResetWave());
    }

    IEnumerator ResetWave()
    {
        _provider.Invoke("OnWaveStart");
        var duration = GetCurrentDuration();
        yield return new WaitForSeconds(duration);
        _provider.Invoke("OnWaveEnd");
        _currentWaveIndex++;
        yield return ResetWave();
    }

    private float GetCurrentDuration()
    {
        var nextWaveIndex = Mathf.Min(_currentWaveIndex, _wavesDuration.Length - 1);
        return _wavesDuration[nextWaveIndex];
    }
}
