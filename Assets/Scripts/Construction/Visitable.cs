using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Visitable : MonoBehaviour
{
    [SerializeField]
    private bool _hasCapacity;

    [EnableIf("_hasCapacity")]
    [SerializeField]
    private int _visitorCapacity;

    private Construction _construction;
    private List<Hunter> _visitedHunters = new();
    private UnityEvent _onVisitorChanged = new UnityEvent();

    public Construction Construction => _construction;
    public bool HasCapacity => _hasCapacity;
    public Hunter[] VisitedHunters => _visitedHunters.ToArray();
    public UnityEvent OnVisitorChanged => _onVisitorChanged;


    private void Awake()
    {
        _construction = GetComponent<Construction>();
    }

    public void EnterVisitor(Hunter hunter)
    {
        if (!_visitedHunters.Contains(hunter))
        {
            if (!_hasCapacity || _visitedHunters.Count < _visitorCapacity)
            {
                _visitedHunters.Add(hunter);
                _onVisitorChanged.Invoke();
            }
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
