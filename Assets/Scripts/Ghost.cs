using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using Zenject;

public class Ghost : CustomUpdatable, IDirectable
{
    public Vector3Int Direction { get { return movementDirection; } set { movementDirection = Direction; }}

    public event Action OnDead;
    public event Action OnFrightened;
    public event Action OnPrevious;

    [SerializeField] protected GameObject savePoint;
    [SerializeField] protected GameObject spawnPoint;

    [Header("Path settings")]
    [SerializeField] protected PathWithoutReturn path;
    [SerializeField] protected bool _routeMustBePainted = true;
    [SerializeField] protected Color _color;

    [Header("State params")]
    public bool IsScattered = true;
    public bool IsFrightened = false;
    public bool IsDead = false;

    protected Tilemap level;
    protected Pacman pacman;
    protected Vector3Int movementDirection = Vector3Int.zero;
    protected EventProvider _provider;
    private Vector3Int _lastPosition;

    [Inject]
    public void Initialize(EventProvider provider, Pacman pacman, [Inject(Id ="level")]Tilemap level)
    {
        _provider = provider;
        this.pacman = pacman;
        this.level = level;
    }

    private void Start()
    {
        _lastPosition = level.WorldToCell(transform.position);
        _provider.Subscribe("OnWaveEnd",ChangeState); 
        _provider.Subscribe("OnAmplificationStart", Fright);
        _provider.Subscribe("OnAmplificationHasEnded",SetPreviousState);
        _provider.Subscribe("OnAmplificationHasEnded", () => IsFrightened = false);
    }

    public void Kill()
    {
        OnDead?.Invoke();
        IsDead = true;
    }

    private void ChangeState()
    {
        IsScattered = !IsScattered;
    }

    public void Fright()
    {
        IsFrightened = true;
        movementDirection = -1 * movementDirection;
        OnFrightened?.Invoke();
    }

    public void SetPreviousState()
    {
        OnPrevious?.Invoke();
    }

    public override void CustomUpdate()
    {
        var currentPosition = level.WorldToCell(transform.position);

        UpdateMovementDirection(currentPosition);

        Vector3 destination = ChooseDestinationPoint(currentPosition);

        var route = path.Create(level.WorldToCell(transform.position),
                                 level.WorldToCell(destination),
                                 movementDirection);
        if (route != null && route.Count > 1)
        {
            transform.position = level.CellToWorld(GetNextPositionInRoute(route));
            if(_routeMustBePainted)
                PathPainter.DrawRoute(route, _color);
        }
    }

    private void UpdateMovementDirection(Vector3Int currentLocalPosition)
    {
        if (currentLocalPosition != _lastPosition)
        {
            movementDirection = currentLocalPosition - _lastPosition;
            _lastPosition = currentLocalPosition;
        }
        Debug.DrawLine(currentLocalPosition, currentLocalPosition + movementDirection, Color.blue, 0.1f, true);
    }

    protected virtual Vector3 ChooseDestinationPoint(Vector3Int position)
    {
        Vector3 destination;
        if (IsDead)
        {
            destination = spawnPoint.transform.position;
            DeathAction();
        }
        else if (IsFrightened)
        {
            destination = path.GetRandomAdjacentNodes(position, movementDirection);
        }
        else if (IsScattered)
        {
            destination = savePoint.transform.position;
        }
        else
        {
            destination = CalculateTargetPosition(pacman);
        }
        return destination;
    }

    protected virtual void DeathAction()
    {        
        Speed = 0.1f;
        if (level.AreOnTheSameTile(gameObject, spawnPoint))
        {
            IsDead = false;
            Speed = 0.3f;
            SetPreviousState();
        }
    }

    protected virtual Vector3 CalculateTargetPosition(Pacman pacman)
    {
        return pacman.transform.position;
    }

    private Vector3Int GetNextPositionInRoute(List<Node> route) => route[1].Position;
}
