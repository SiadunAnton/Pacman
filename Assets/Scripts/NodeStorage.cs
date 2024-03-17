using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3Int Position;
    public int Cost = int.MaxValue;
    public Node Previous = null;

    public Node(Vector3Int position) => Position = position;
}

public class NodeStorage
{
    private Dictionary<Vector3Int, Node> _storage = new Dictionary<Vector3Int, Node>();

    public Node Get(Vector3Int position)
    {
        if (!_storage.ContainsKey(position))
        {
            var node = new Node(position);
            _storage.Add(position, node);
        }
        return _storage[position];
    }
}