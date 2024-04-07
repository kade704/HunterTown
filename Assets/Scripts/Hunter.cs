using System.Collections;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private float _defaultHp;
    [SerializeField] private float _defaultDamage;
    private bool _isDispatched = false;


    private SpriteRenderer _spriteRenderer;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public float DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public float DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public Sprite Sprite => _spriteRenderer.sprite;
    public bool IsDispatched { get { return _isDispatched; } set { _isDispatched = value; } }

    public bool Dispatch
    {
        set
        {
            _isDispatched = value;
            _spriteRenderer.enabled = value;
        }
    }

    public float Viability
    {
        get
        {
            return DefaultHp + DefaultDamage * 0.8f;
        }
    }

    public float CombatPower
    {
        get
        {
            return DefaultDamage * 1.2f + DefaultHp * 0.5f;
        }
    }

    private void Awake()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var _roads = ConstructionManager.Instance.Constructions.FilterCast<Road>().ToArray();

        Road start = null;
        if (_roads.Length > 0)
        {
            start = _roads[Random.Range(0, _roads.Length)];
            transform.position = start.transform.position;
        }
        StartCoroutine(MoveRoutine(start));
    }

    private void Update()
    {
        _spriteRenderer.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);

        _spriteRenderer.enabled = !_isDispatched;
    }

    private IEnumerator MoveRoutine(Road start)
    {
        var _roads = ConstructionManager.Instance.Constructions.FilterCast<Road>().ToArray();
        Road target = null;
        if (_roads.Length > 0)
        {
            target = _roads[Random.Range(0, _roads.Length)];

            var path = PathFinder.SearchPath(start, target, _roads);

            int index = 0;

            while (index < path.Length)
            {
                while (Vector3.Distance(transform.position, path[index].transform.position) > 0.01)
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[index].transform.position, Time.deltaTime);
                    yield return null;
                }
                index++;
            }
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(MoveRoutine(target));
    }
}
