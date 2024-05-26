using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Visitable : MonoBehaviour
{
    [SerializeField]
    private int _visitorCapacity;

    private List<Hunter> _visitedHunters = new();
    private UnityEvent _onVisitorChanged = new UnityEvent();

    public Hunter[] VisitedHunters => _visitedHunters.ToArray();
    public UnityEvent OnVisitorChanged => _onVisitorChanged;


    public void EnterVisitor(Hunter hunter)
    {
        if (!_visitedHunters.Contains(hunter) && _visitedHunters.Count < _visitorCapacity)
        {
            _visitedHunters.Add(hunter);
            _onVisitorChanged.Invoke();
        }
    }

    public void ExitVisitor(Hunter hunter)
    {
        if (_visitedHunters.Contains(hunter))
        {
            _visitedHunters.Remove(hunter);
            _onVisitorChanged.Invoke();
        }
    }
}
