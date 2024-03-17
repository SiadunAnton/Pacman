using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PacmanInteractionProvider : MonoBehaviour
{
    public bool IsAmplified => _isAmplified;

    [SerializeField] private List<FrightenTrigger> _powerUpCollection;

    [Space]
    [Header("Speed settings")]
    [Min(0f)] [SerializeField] private float _normalSpeed;
    [Min(0f)] [SerializeField] private float _amplifiedSpeed;

    private Tilemap _level;
    private Pacman _pacman;
    private EventProvider _provider;
    private Ghost[] _ghosts;
    private bool _isAmplified = false;
    private bool _isDead = false;

    [Inject]
    public void Initialize(EventProvider provider, [Inject(Id = "all")] Ghost[] ghosts,
                            [Inject(Id = "level")] Tilemap level, Pacman pacman)
    {
        _provider = provider;
        _ghosts = ghosts;
        _level = level;
        _pacman = pacman;
    }

    private void Awake()
    {
        _provider.Register("OnPacmanDie");
        _provider.Register("OnAmplificationStart");
        _provider.Register("OnAmplificationHasEnded");
        _provider.Register("OnPacmanKillGhost");
        _pacman.Speed = _normalSpeed;
    }

    public void Update()
    {
        CheckPositionRelativeToPowerUps();
        CheckPositionRelativeToGhosts();
    }

    private void CheckPositionRelativeToPowerUps()
    {
        foreach (var powerUp in _powerUpCollection)
            if (IsFrightTriggerCanBeEnabled(powerUp))
            {
                powerUp.Hide();
                StopAllCoroutines();
                StartCoroutine(BoostPacman());
            }
    }

    private bool IsFrightTriggerCanBeEnabled(FrightenTrigger trigger)
    {
        return IsObjectOnTheSameTileWithPacman(trigger.gameObject) && trigger.isActive;
    }

    IEnumerator BoostPacman()
    {
        _isAmplified = true;
        _pacman.Speed = _amplifiedSpeed;
        _provider.Invoke("OnAmplificationStart");
        yield return new WaitForSecondsRealtime(10f);
        _pacman.Speed = _normalSpeed;
        _isAmplified = false;
        _provider.Invoke("OnAmplificationHasEnded");
    }

    private void CheckPositionRelativeToGhosts()
    {
        foreach (var ghost in _ghosts)
            if (IsObjectOnTheSameTileWithPacman(ghost.gameObject))
            {
                if (ghost.IsDead)
                    return;

                if (_isAmplified && ghost.IsFrightened)
                {
                    _provider.Invoke("OnPacmanKillGhost");
                    ghost.Kill();
                    Score.Value += 100;
                }
                else
                {
                    if (_isDead == false)
                    {
                        _isDead = true;
                        _provider.Invoke("OnPacmanDie");
                    }
                }
            }
    }

    private bool IsObjectOnTheSameTileWithPacman(GameObject anObject)
    {
        return _level.AreOnTheSameTile(anObject, _pacman.gameObject);
    }
}