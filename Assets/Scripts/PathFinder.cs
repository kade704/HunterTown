using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public class Node
    {
        public Vector2Int Position { get; }
        public int Cost { get; set; }
        public bool Visited { get; set; }
        public Node Parent { get; set; }
        public Construction Construction { get; set; }

        public Node(Vector2Int position, Construction construction)
        {
            Position = position;
            Cost = int.MaxValue;
            Visited = false;
            Parent = null;
            Construction = construction;
        }

        public void Reset()
        {
            Cost = int.MaxValue;
            Visited = false;
            Parent = null;
        }
    }

    private Dictionary<Vector2Int, Node> _nodeCache = new();

    private void Start()
    {
        InitializeNodes();

        var constructionGridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();
        constructionGridmap.OnConstructionBuilded.AddListener((construction) =>
        {
            _nodeCache[construction.CellPos].Construction = construction;
        });
        constructionGridmap.OnConstructionDestroyed.AddListener((construction) =>
        {
            _nodeCache[construction.CellPos].Construction = null;
        });
    }

    private void InitializeNodes()
    {
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                var position = new Vector2Int(i, j);
                var construction = GameManager.Instance.GetSystem<ConstructionGridmap>().GetConstructionAt(position);
                _nodeCache[position] = new Node(position, construction);
            }
        }
    }

    public Path SearchPath(Vector2Int start, Vector2Int end)
    {
        _nodeCache.Values.ForEach(node => node.Reset());

        var queue = new PriorityQueue<Node>();

        var startNode = _nodeCache[start];
        startNode.Cost = 0;
        queue.Enqueue(startNode, Heuristic(start, end));

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            var current = currentNode.Position;

            if (_nodeCache[current].Visited)
            {
                continue;
            }

            currentNode.Visited = true;
            _nodeCache[current] = currentNode;

            if (current == end)
            {
                return ConstructPath(currentNode, start);
            }

            ExploreNeighbors(queue, currentNode, end);
        }

        return null;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private void ExploreNeighbors(PriorityQueue<Node> queue, Node currentNode, Vector2Int end)
    {
        var x = new[] { -1, 1, 0, 0 };
        var y = new[] { 0, 0, -1, 1 };
        for (int i = 0; i < 4; i++)
        {
            var cellPos = new Vector2Int(currentNode.Position.x + x[i], currentNode.Position.y + y[i]);

            if (!_nodeCache.ContainsKey(cellPos) || _nodeCache[cellPos].Visited)
            {
                continue;
            }

            int weight;
            var construction = _nodeCache[cellPos].Construction;

            if (construction == null)
            {
                weight = 100;
            }
            else if (construction.GetComponent<Road>() != null)
            {
                weight = 10 - (int)construction.GetComponent<Road>().Speed;
            }
            else if (construction.CellPos == end || construction.CellPos == currentNode.Position)
            {
                weight = 1;
            }
            else
            {
                continue;
            }

            var neighborNode = _nodeCache[cellPos];
            var newCost = currentNode.Cost + weight;

            if (newCost < neighborNode.Cost)
            {
                neighborNode.Cost = newCost;
                neighborNode.Parent = currentNode;

                _nodeCache[cellPos] = neighborNode;
                queue.Enqueue(neighborNode, newCost + Heuristic(cellPos, end));
            }
        }
    }

    private static Path ConstructPath(Node endNode, Vector2Int start)
    {
        var positions = new List<Vector2Int>();
        var currentNode = endNode;

        while (currentNode.Parent != null)
        {
            positions.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        positions.Add(start);
        positions.Reverse();

        var nodes = new List<Path.Node>();
        for (var i = 0; i < positions.Count - 1; i++)
        {
            var nextPos = positions[i + 1];
            var direction = Direction.None;
            if (positions[i].x < nextPos.x) direction = Direction.East;
            else if (positions[i].x > nextPos.x) direction = Direction.West;
            else if (positions[i].y < nextPos.y) direction = Direction.North;
            else if (positions[i].y > nextPos.y) direction = Direction.South;
            nodes.Add(new Path.Node(positions[i], direction));
        }
        nodes.Add(new Path.Node(positions[^1], Direction.None));

        return new Path(nodes.ToArray());
    }
}