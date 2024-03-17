using UnityEngine;

public class Speedy : Ghost
{
    protected override Vector3 CalculateTargetPosition(Pacman pacman)
    {
        var targetPos = level.CellToWorld(level.WorldToCell(pacman.transform.position) + 4 * pacman.Direction);
        return targetPos;
    }
}
