using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class PathWithoutReturn : MonoBehaviour
{
    [SerializeField] private Tilemap _level;
    [SerializeField] private RuleTile _wall;

    public List<Node> Create(Vector3Int start, Vector3Int end, Vector3Int startDirection)
    {
        if (start == end)
            return null;

        var storage = new NodeStorage();

        List<Node> reachable = new List<Node>();
        List<Node> explored = new List<Node>();
        var backwards = storage.Get(start + -1 * startDirection);
        explored.Add(backwards);
        var arrivePoint = storage.Get(start);
        reachable.Add(arrivePoint);
        var destination = storage.Get(end);

        int counter = 0;

        while (reachable.Count != 0)
        {
            var node = ChooseNode(reachable, destination);

            if (node.Position == destination.Position)
                return BuildPathTo(destination);

            reachable.Remove(node);
            explored.Add(node);

            var newReachable = GetAdjacentNodes(node, storage).Except(explored);

            foreach (var adjacent in newReachable)
            {
                if (!reachable.Contains(adjacent))
                {
                    reachable.Add(adjacent);
                }

                if (node.Cost + 1 < adjacent.Cost)
                {
                    adjacent.Previous = node;
                    adjacent.Cost = node.Cost + 1;
                }
            }

            if (counter == 2000)
            {
                throw new System.Exception("Reached circular dependency.");
            }
            counter++;
        }

        explored.Remove(arrivePoint);
        explored.Remove(backwards);
        var nearest = ChooseNearestNode(explored, destination);
        return BuildPathTo(nearest);
    }

    private Node ChooseNode(List<Node> graphCollection, Node target)
    {
        Node bestNode = null;
        int minCost = int.MaxValue;

        foreach (var node in graphCollection)
        {
            var costFromStartToNode = node.Cost;
            var costFromNodeToEnd = EstimateDistance(node, target);
            var totalCost = costFromStartToNode + costFromNodeToEnd;

            if (minCost > totalCost)
            {
                minCost = totalCost;
                bestNode = node;
            }
        }

        return bestNode;
    }

    private Node ChooseNearestNode(List<Node> graphCollection, Node target)
    {
        Node bestNode = null;
        int minCost = int.MaxValue;

        foreach (var node in graphCollection)
        {
            var totalCost = EstimateDistance(node, target);

            if (minCost > totalCost)
            {
                minCost = totalCost;
                bestNode = node;
            }
        }

        return bestNode;
    }

    private int EstimateDistance(Node start, Node end)
    {
        return Mathf.Abs(start.Position.x - end.Position.x) +
            Mathf.Abs(start.Position.y - end.Position.y);
    }

    private List<Node> BuildPathTo(Node target)
    {
        List<Node> path = new List<Node>();
        var node = target;
        while (node != null)
        {
            path.Add(node);
            node = node.Previous;
        }
        path.Reverse();
        return path;
    }

    private IEnumerable<Node> GetAdjacentNodes(Node node, NodeStorage storage)
    {
        var directions = new List<Vector3Int>{ Vector3Int.up, Vector3Int.left, Vector3Int.down, Vector3Int.right};
        var result = new List<Node>();

        foreach (var direction in directions)
            result.Add(storage.Get(node.Position + direction));

        return result.Where(x => _level.GetTile(x.Position) != _wall);
    }

    public Vector3Int GetRandomAdjacentNodes(Vector3Int position, Vector3Int startDirection)
    {
        var result = new List<Node>();
        try
        {
            NodeStorage storage = new NodeStorage();
            var directions = new List<Vector3Int> { Vector3Int.up, Vector3Int.left, Vector3Int.down, Vector3Int.right };
            directions.Remove(-1 * startDirection);


            foreach (var direction in directions)
                result.Add(storage.Get(position + direction));

            result = result.Where(x => _level.GetTile(x.Position) != _wall).ToList();
            return result[Random.Range(0, result.Count)].Position;
        }
        catch
        {
            Debug.Log($"result size: {result.Count},");
        }
        return Vector3Int.zero;
        
    }
}
