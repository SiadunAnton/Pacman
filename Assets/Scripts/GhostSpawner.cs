using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnRoute;
    [SerializeField] private float _gap;
    [SerializeField] private float _moveSpeed = 1f;

    private Ghost[] _ghosts;

    [Inject]
    public void Initialize([Inject(Id ="3last")] Ghost[] ghosts)
    {
        _ghosts = ghosts;
    }

    private void Start()
    {
        foreach (var ghost in _ghosts)
            ghost.enabled = false;

        StartCoroutine(SpawnProcess());
    }

    IEnumerator SpawnProcess()
    {
        for(int i = 0; i < _ghosts.Length; i++)
        {
            yield return new WaitForSeconds(_gap);
            yield return MoveGhostToStartPoint(_ghosts[i]);
            _ghosts[i].enabled = true;
        }
    }

    IEnumerator MoveGhostToStartPoint(CustomUpdatable ghost)
    {
        foreach(var point in _spawnRoute)
        {
            ghost.transform.DOMove(point.position,_moveSpeed);
            yield return new WaitForSeconds(_moveSpeed);
        }
    }
}
