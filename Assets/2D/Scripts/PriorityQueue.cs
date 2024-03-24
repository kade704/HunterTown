using System;
using System.Collections.Generic;

public class PriorityQueue<T> {
    private List<Tuple<T, float>> elements = new();

    public int Count {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priorityValue) {
        elements.Add(Tuple.Create(item, priorityValue));
    }

    public T Dequeue() {
        int bestPriorityIndex = 0;

        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].Item2 < elements[bestPriorityIndex].Item2) {
                bestPriorityIndex = i;
            }
        }

        T bestItem = elements[bestPriorityIndex].Item1;
        elements.RemoveAt(bestPriorityIndex);
        return bestItem;
    }

    public T Peek() {
        int bestPriorityIndex = 0;

        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].Item2 < elements[bestPriorityIndex].Item2) {
                bestPriorityIndex = i;
            }
        }

        T bestItem = elements[bestPriorityIndex].Item1;
        return bestItem;
    }
}
