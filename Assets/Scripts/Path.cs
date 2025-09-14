using UnityEngine;

public class Path
{
    public class Node
    {
        private Vector2Int _position;
        private Direction _direction;

        public Node(Vector2Int position, Direction direction)
        {
            _position = position;
            _direction = direction;
        }

        public Vector2Int Position => _position;

        public Direction Direction => _direction;
    }

    private Node[] _nodes;
    private int _location;

    public Path(Node[] nodes)
    {
        _nodes = nodes;
        _location = 0;
    }

    public Node[] Nodes => _nodes;

    public int Length => _nodes.Length;
    public int Location
    {
        get => _location;
        set => _location = value;
    }
}
