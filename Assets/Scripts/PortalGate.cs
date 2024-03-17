using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class PortalGate : MonoBehaviour
{
    [Header("Endings")]
    [SerializeField] private Transform _leftGate;
    [SerializeField] private Transform _rightGate;

    [Header("Interaction objects")]
    [SerializeField] private List<GameObject> _targets;

    private Tilemap _level;

    [Inject]
    public void Initialize([Inject(Id ="level")] Tilemap level)
    {
        _level = level;
    }

    private void Update()
    {
        foreach(var anObject in _targets)
        {
            if(_level.WorldToCell(anObject.transform.position) == _level.WorldToCell(_leftGate.transform.position))
            {
                Move(anObject, _level.CellToWorld(_level.WorldToCell(_rightGate.transform.position)+Vector3Int.left));
            }
            else if(_level.WorldToCell(anObject.transform.position) == _level.WorldToCell(_rightGate.transform.position))
            {
                Move(anObject, _level.CellToWorld(_level.WorldToCell(_leftGate.transform.position) + Vector3Int.right));
            }
        }
    }

    private void Move(GameObject target,Vector3 finish)
    {
        target.transform.position = finish;
    }
}
