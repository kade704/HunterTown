using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    readonly List<Tuple<T, double>> _elements = new();

    public int Count => _elements.Count;

    public void Enqueue(T item, double priorityValue)
    {
        _elements.Add(Tuple.Create(item, priorityValue));
    }

    public T Dequeue()
    {
        var bestPriorityIndex = 0;

        for (int i = 0; i < _elements.Count; i++)
        {
            if (_elements[i].Item2 < _elements[bestPriorityIndex].Item2)
            {
                bestPriorityIndex = i;
            }
        }

        var bestItem = _elements[bestPriorityIndex].Item1;
        _elements.RemoveAt(bestPriorityIndex);
        return bestItem;
    }

    public T Peek()
    {
        var bestPriorityIndex = 0;

        for (int i = 0; i < _elements.Count; i++)
        {
            if (_elements[i].Item2 < _elements[bestPriorityIndex].Item2)
            {
                bestPriorityIndex = i;
            }
        }

        var bestItem = _elements[bestPriorityIndex].Item1;
        return bestItem;
    }
}