using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Interactable))]
public class Hunter : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private float _defaultHp;
    [SerializeField] private float _defaultDamage;
    private bool _isDispatched = false;
    private Interactable _interactable;
    private SpriteRenderer _bodySprite;
    private SpriteRenderer _hairSprite;
    private SpriteRenderer _leftSleeveSprite;
    private SpriteRenderer _rightSleeveSprite;
    private SpriteRenderer _leftPantSprite;
    private SpriteRenderer _rightPantSprite;
    private SortingGroup _sortingGroup;
    private Transform _spriteRoot;
    private Animator _animator;
    private Sprite _thumbnail;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public float DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public float DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public bool IsDispatched { get { return _isDispatched; } set { _isDispatched = value; } }
    public Sprite Thumbnail { get { return _thumbnail; } set { _thumbnail = value; } }
    public Sprite BodySprite { get { return _bodySprite.sprite; } set { _bodySprite.sprite = value; } }
    public Sprite HairSprite { get { return _hairSprite.sprite; } set { _hairSprite.sprite = value; } }
    public Sprite LeftSleeveSprite { get { return _leftSleeveSprite.sprite; } set { _leftSleeveSprite.sprite = value; } }
    public Sprite RightSleeveSprite { get { return _rightSleeveSprite.sprite; } set { _rightSleeveSprite.sprite = value; } }
    public Sprite LeftPantSprite { get { return _leftPantSprite.sprite; } set { _leftPantSprite.sprite = value; } }
    public Sprite RightPantSprite { get { return _rightPantSprite.sprite; } set { _rightPantSprite.sprite = value; } }
    public Color HairColor { get { return _hairSprite.color; } set { _hairSprite.color = value; } }

    public bool Dispatch
    {
        set => _isDispatched = value;
    }

    public float Viability
    {
        get => DefaultHp + DefaultDamage * 0.8f;
    }

    public float CombatPower
    {
        get => DefaultDamage * 1.2f + DefaultHp * 0.5f;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _interactable = GetComponent<Interactable>();
        _sortingGroup = GetComponent<SortingGroup>();

        _spriteRoot = transform.Find("Root");
        _bodySprite = transform.Find("Root/BodySet/P_Body/Body/P_ClothBody/ClothBody").GetComponent<SpriteRenderer>();
        _hairSprite = transform.Find("Root/BodySet/P_Body/HeadSet/P_Head/P_Hair/7_Hair").GetComponent<SpriteRenderer>();
        _leftSleeveSprite = transform.Find("Root/BodySet/P_Body/ArmSet/ArmL/P_LArm/P_Arm/20_L_Arm/P_LCArm/21_LCArm").GetComponent<SpriteRenderer>();
        _rightSleeveSprite = transform.Find("Root/BodySet/P_Body/ArmSet/ArmR/P_RArm/P_Arm/-20_R_Arm/P_RCArm/-19_RCArm").GetComponent<SpriteRenderer>();
        _leftPantSprite = transform.Find("Root/P_LFoot/P_LCloth/_4L_Cloth").GetComponent<SpriteRenderer>();
        _rightPantSprite = transform.Find("Root/P_RFoot/P_RCloth/_9R_Cloth").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _interactable.DisplayName = _displayName;


        CaptureThumbnailRoutine();
        StartCoroutine(MoveRoutine());
        StartCoroutine(CaptureThumbnailRoutine());
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);
        _spriteRoot.gameObject.SetActive(!_isDispatched);
    }

    private IEnumerator MoveRoutine()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();
        var roads = FindObjectsOfType<Road>();
        Road start = roads[Random.Range(0, roads.Length)];
        transform.position = start.transform.position;

        while (true)
        {
            var moveTime = timeSystem.Hour.Total + Random.Range(5, 15);
            while (timeSystem.Hour.Total < moveTime)
            {
                yield return null;
            }

            roads = FindObjectsOfType<Road>();
            if (roads.Length == 0) continue;

            Road target = roads[Random.Range(0, roads.Length)];

            var path = SearchPath(start, target);
            if (path == null) continue;

            int index = 0;

            while (index < path.Length)
            {
                if (!path[index]) break;

                while (Vector2.Distance(transform.position, path[index].transform.position) > 0.1f)
                {
                    var speed = timeSystem.TimeScale * 0.6f;
                    var dir = (path[index].transform.position - transform.position).normalized;
                    var velocity = speed * Time.deltaTime * dir;

                    _spriteRoot.localScale = new Vector3(velocity.x < 0 ? 0.5f : -0.5f, 0.5f, 1);

                    transform.position += velocity;

                    _animator.SetFloat("RunState", 0.5f);
                    yield return null;
                }
                index++;
            }
            start = target;

            _animator.SetFloat("RunState", 0);

        }
    }

    public static Road[] SearchPath(Road start, Road end)
    {
        static int heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        var costs = new Dictionary<Road, int>();
        var queue = new PriorityQueue<Road>();
        var visited = new HashSet<Road>();
        var parent = new Dictionary<Road, Road>();

        queue.Enqueue(start, heuristic(start.Construction.CellPos, end.Construction.CellPos));
        costs[start] = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var cost = costs.ContainsKey(current) ? costs[current] : int.MaxValue;
            visited.Add(current);

            if (current == end)
            {
                var path = new List<Road>();
                while (current != start)
                {
                    path.Add(current);
                    current = parent[current];
                }
                path.Add(start);
                path.Reverse();
                return path.ToArray();
            }

            foreach (var neighbor in current.Neighbors.FilterCast<Road>())
            {
                if (visited.Contains(neighbor)) continue;

                var newCost = cost + 1 + heuristic(neighbor.Construction.CellPos, end.Construction.CellPos);
                queue.Enqueue(neighbor, newCost);
                costs[neighbor] = newCost;
                parent[neighbor] = current;
            }
        }

        return null;
    }

    public IEnumerator CaptureThumbnailRoutine(int resolution = 512)
    {
        var position = transform.position;
        transform.position = new Vector3(Random.Range(456, 789), Random.Range(456, 789), 0);

        yield return null;

        var renderTexture = RenderTexture.GetTemporary(resolution, resolution, 16, RenderTextureFormat.ARGB32);

        var camera = new GameObject("Camera").AddComponent<Camera>();
        camera.transform.parent = transform;
        camera.transform.localPosition = new Vector3(0, 0.2f, -10);
        camera.orthographic = true;
        camera.orthographicSize = 0.3f;
        camera.cullingMask = 1 << LayerMask.NameToLayer("Hunter");
        camera.targetTexture = renderTexture;
        camera.Render();

        Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);

        var previousRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        RenderTexture.ReleaseTemporary(renderTexture);
        camera.targetTexture = null;
        RenderTexture.active = previousRenderTexture;

        texture.Apply();

        Destroy(camera.gameObject);

        _thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        transform.position = position;
    }
}
