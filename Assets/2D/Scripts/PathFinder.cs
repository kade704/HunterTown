using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder {
    private class Node {
        public Road road;
        public Node parent;
        public List<Node> neighbors;
        public int g;
        public int h;
    }

    private static Node[] SearchPath(Node start, Node end) {
        Func<Vector2Int, Vector2Int, int> heuristic = (a, b) => {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        };

        var openSet = new List<Node>();
        var closeSet = new List<Node>();
        openSet.Add(start);

        while (openSet.Count > 0) {
            openSet = openSet.OrderBy(x => x.g + x.h).ToList();
            var node = openSet.First();
            openSet.RemoveAt(0);
            closeSet.Add(node);

            if (node == end) {
                var path = new List<Node>();
                while (node != start) {
                    path.Add(node);
                    node = node.parent;
                }
                path.Reverse();
                return path.ToArray();
            }

            foreach (var neighbor in node.neighbors) {
                if (closeSet.Contains(neighbor)) {
                    continue;
                }

                int cost = node.g + heuristic(node.road.CellPos, neighbor.road.CellPos);
                if (cost < neighbor.g || !openSet.Contains(neighbor)) {
                    neighbor.g = cost;
                    neighbor.h = heuristic(neighbor.road.CellPos, end.road.CellPos);
                    neighbor.parent = node;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new Node[0];
    }
    public static Road[] SearchPath(Road start, Road end, Road[] roads) {
        var nodes = new List<Node>();
        foreach (var road in roads) {
            nodes.Add(new Node {
                road = road,
                parent = null,
                neighbors = new(),
                g = 0,
                h = 0
            });
        }

        foreach (var node in nodes) {
            var xPos = new int[] { 1, -1, 0, 0 };
            var yPos = new int[] { 0, 0, 1, -1 };

            for (int i = 0; i < 4; i++) {
                var neighbor = nodes.Where(x => x.road.CellPos == new Vector2Int(node.road.CellPos.x + xPos[i], node.road.CellPos.y + yPos[i])).FirstOrDefault();
                if (neighbor != null) {
                    node.neighbors.Add(neighbor);
                }
            }
        }

        var startNode = nodes.Find(x => x.road == start);
        var endNode = nodes.Find(x => x.road == end);

        var path = SearchPath(startNode, endNode);
        return path.Select(x => x.road).ToArray();
    }
}
