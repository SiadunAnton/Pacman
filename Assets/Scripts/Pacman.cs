using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class Pacman : CustomUpdatable, IDirectable
{
    public Vector3Int Direction { get { return _direction; } set { _direction = value; } }
    public Vector3Int _direction = Vector3Int.right;

    [SerializeField] private RuleTile _tile;

    private Tilemap _level;

    [Inject]
    public void Initialize([Inject(Id ="level")] Tilemap level)
    {
        _level = level;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (IsMovementInDirectionAllowed(Vector3Int.right))
                Direction = Vector3Int.right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (IsMovementInDirectionAllowed(Vector3Int.left))
                Direction = Vector3Int.left;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (IsMovementInDirectionAllowed(Vector3Int.up))
                Direction = Vector3Int.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (IsMovementInDirectionAllowed(Vector3Int.down))
                Direction = Vector3Int.down;
        }
    }

    public override void CustomUpdate()
    {
        var celledPosition = _level.WorldToCell(transform.position);
        var nextPosition = celledPosition + Direction;

        if (_level.GetTile(nextPosition) != _tile)
        {
            transform.position = _level.CellToWorld(nextPosition);
        }
    }

    private bool IsMovementInDirectionAllowed(Vector3Int direction)
    {
        var position = _level.WorldToCell(transform.position);
        var nextPosition = position + direction;
        return _level.GetTile(nextPosition) != _tile; 
    }
}
