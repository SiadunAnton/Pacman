using System.Collections;
using UnityEngine;
using Zenject;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip _start;
    [SerializeField] private AudioClip _munch;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _eated;
    [SerializeField] private AudioClip _fright;

    private EventProvider _provider;
    private AudioSource _source;
    private Pause _pause;

    [Inject]
    public void Initialize(EventProvider provider,Pause pause)
    {
        _provider = provider;
        _pause = pause;
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Play(_start);

        _provider.Subscribe("OnPacmanDie",PlayDeathSound);
        _provider.Subscribe("OnAmplificationStart",PlayFrightenSound);
        _provider.Subscribe("OnPacmanKillGhost",PlayFrightenSound);
    }

    public void PlayFrightenSound()
    {
        Play(_fright);
    }

    public void PlayDeathSound()
    {
        Play(_death);
    }

    public void PlayEatedSound()
    {
        Play(_eated);
    }

    private void Play(AudioClip clip)
    {
        StopAllCoroutines();
        StartCoroutine(PlayProcess(clip));
    }

    IEnumerator PlayProcess(AudioClip clip)
    {
        _pause.Run(clip.length);
        _source.Stop();
        var previousLoopingState = _source.loop;
        _source.loop = false;
        _source.clip = clip;
        _source.Play();
        yield return new WaitForSeconds(clip.length);
        _source.loop = previousLoopingState;
        _source.clip = _munch;
        _source.Play();
    }

    private void OnDestroy()
    {
        _source.Stop();
    }
}
