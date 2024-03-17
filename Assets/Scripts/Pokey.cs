using UnityEngine;

public class Pokey : Ghost
{
    protected override Vector3 ChooseDestinationPoint(Vector3Int position)
    {
        Vector3 destination;

        var distanceToPacman = Vector3Int.Distance(level.WorldToCell(pacman.transform.position),
                                                      position);
        if (IsDead)
        {
            destination = spawnPoint.transform.position;
            DeathAction(); 
        }
        else if (IsFrightened)
        {
            destination = path.GetRandomAdjacentNodes(position, movementDirection);
        }
        else if (IsScattered || distanceToPacman <= 8f)
        {
            destination = savePoint.transform.position;
        }
        else
        {
            destination = CalculateTargetPosition(pacman);
        }
        return destination;
    }
}
