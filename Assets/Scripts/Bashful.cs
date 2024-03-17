using UnityEngine;
using Zenject;

public class Bashful : Ghost
{
    private Ghost _shadow;

    [Inject]
    public void InitializeAdditionalTarget([Inject(Id ="shadow")] Ghost shadow)
    {
        _shadow = shadow;
    }

    protected override Vector3 CalculateTargetPosition(Pacman pacman)
    {
        var currentPos = level.WorldToCell(pacman.transform.position) + 2 * pacman.Direction;
        var targetPos = 2 * currentPos - level.WorldToCell(_shadow.transform.position);
        return targetPos;
    }
}
