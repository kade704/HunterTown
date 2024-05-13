using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public static Path SearchPath(Vector2Int start, Vector2Int end)
    {
        static int heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        var costs = new Dictionary<Vector2Int, int>();
        var queue = new PriorityQueue<Vector2Int>();
        var visited = new HashSet<Vector2Int>();
        var parent = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start, heuristic(start, end));
        costs[start] = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var cost = costs.ContainsKey(current) ? costs[current] : int.MaxValue;
            visited.Add(current);

            if (current == end)
            {
                var positions = new List<Vector2Int>();
                while (current != start)
                {
                    positions.Add(current);
                    current = parent[current];
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

            var x = new[] { -1, 1, 0, 0 };
            var y = new[] { 0, 0, -1, 1 };
            for (int i = 0; i < 4; i++)
            {
                var cellPos = new Vector2Int(current.x + x[i], current.y + y[i]);

                if (visited.Contains(cellPos)) continue;

                int weight;

                var construction = GameManager.Instance.GetSystem<ConstructionGridmap>().GetConstructionAt(cellPos);
                if (construction == null)
                {
                    weight = 10;
                }
                else if (construction.GetComponent<Road>() != null || construction.CellPos == end || construction.CellPos == start)
                {
                    weight = 1;
                }
                else
                {
                    continue;
                }

                var newCost = cost + weight + heuristic(cellPos, end);
                queue.Enqueue(cellPos, newCost);
                costs[cellPos] = newCost;
                parent[cellPos] = current;
            }
        }

        return null;
    }
}
