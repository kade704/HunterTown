using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private readonly Dictionary<string, MonoBehaviour> _systems = new();

    public static GameManager Instance => _instance;

    public T GetSystem<T>() where T : MonoBehaviour
    {
        var systemType = typeof(T).Name;
        if (_systems.ContainsKey(systemType))
        {
            if (!_systems[systemType])
            {
                _systems[systemType] = FindObjectOfType<T>();
            }
            return _systems[systemType] as T;
        }
        else
        {
            var system = FindObjectOfType<T>();
            _systems.Add(systemType, system);
            return system;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
