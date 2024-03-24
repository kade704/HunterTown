using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder {
    void SearchPath(Vector2Int start, Vector2Int end) {
        Func<Vector2Int, Vector2Int, int> heuristic = (a, b) => {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        };

        var visited = new List<Vector2Int>();
        var priorityQueue = new PriorityQueue<Vector2Int>();
        visited.Add(start);
        priorityQueue.Enqueue(start, heuristic(start, end));

        //while (priorityQueue.Count > 0) {
        //    priorityQueue = priorityQueue.OrderBy(x => x.Item2).ToList();
        //    var node = priorityQueue.First();
        //    priorityQueue.Remove(node);

        //    var eastSide = new Vector2Int(node.Item1.x + 1, node.Item1.y);
        //    if (!visited.Contains(eastSide)) {
        //        visited.Add(eastSide);
        //        queue.Enqueue(eastSide);
        //    }

        //    var westSide = new Vector2Int(node.Item1.x - 1, node.Item1.y);
        //    if (!visited.Contains(westSide)) {
        //        visited.Add(westSide);
        //        queue.Enqueue(westSide);
        //    }

        //    var southSide = new Vector2Int(node.Item1.x, node.Item1.y - 1);
        //    if (!visited.Contains(southSide)) {
        //        visited.Add(southSide);
        //        queue.Enqueue(southSide);
        //    }

        //    var northSide = new Vector2Int(node.Item1.x, node.y + 1);
        //    if (!visited.Contains(northSide)) {
        //        visited.Add(northSide);
        //        queue.Enqueue(northSide);
        //    }
    }
}
