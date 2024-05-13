using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(Interactable))]
public class Hunter : MonoBehaviour
{
    [ReadOnly][SerializeField] private string _displayName;
    [ReadOnly][SerializeField] private float _defaultHp;
    [ReadOnly][SerializeField] private float _defaultDamage;
    [SerializeField] private SpriteRenderer _bodyClothRenderer;
    [SerializeField] private SpriteRenderer _hairRenderer;
    [SerializeField] private SpriteRenderer _leftSleeveRenderer;
    [SerializeField] private SpriteRenderer _rightSleeveRenderer;
    [SerializeField] private SpriteRenderer _leftPantRenderer;
    [SerializeField] private SpriteRenderer _rightPantRenderer;
    [SerializeField] private Transform _spriteRoot;
    [SerializeField] private SpriteRenderer _shadowRenderer;


    private Interactable _interactable;
    private SortingGroup _sortingGroup;

    private Animator _animator;
    private Sprite _thumbnail;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public float DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public float DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public Sprite Thumbnail { get { return _thumbnail; } set { _thumbnail = value; } }
    public Sprite BodyClothSprite { get { return _bodyClothRenderer.sprite; } set { _bodyClothRenderer.sprite = value; } }
    public Sprite HairSprite { get { return _hairRenderer.sprite; } set { _hairRenderer.sprite = value; } }
    public Sprite LeftSleeveSprite { get { return _leftSleeveRenderer.sprite; } set { _leftSleeveRenderer.sprite = value; } }
    public Sprite RightSleeveSprite { get { return _rightSleeveRenderer.sprite; } set { _rightSleeveRenderer.sprite = value; } }
    public Sprite LeftPantSprite { get { return _leftPantRenderer.sprite; } set { _leftPantRenderer.sprite = value; } }
    public Sprite RightPantSprite { get { return _rightPantRenderer.sprite; } set { _rightPantRenderer.sprite = value; } }
    public Color HairColor { get { return _hairRenderer.color; } set { _hairRenderer.color = value; } }


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
    }

    private void Start()
    {
        _interactable.DisplayName = _displayName;
        _interactable.Description = $"방어력: {Viability}\n공격력: {CombatPower}";

        _interactable.OnInteracted.AddListener((interaction) =>
        {
            if (interaction.ID == "#move_target")
            {
                _animator.SetFloat("RunState", 0);

                StopAllCoroutines();
                StartCoroutine(MoveTargetRoutine());
            }
        });

        StartCoroutine(WanderRoutine());
        StartCoroutine(CaptureThumbnailRoutine());
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + 1;
    }

    private IEnumerator WanderRoutine()
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

            var target = roads[Random.Range(0, roads.Length)].Construction.CellPos;
            yield return MoveRoutine(target);
        }
    }

    public IEnumerator MoveRoutine(Vector2Int target, bool drawPath = false)
    {
        var start = GameManager.Instance.GetSystem<ConstructionGridmap>().WorldToCell(transform.position);

        var path = PathFinder.SearchPath(start, target);
        if (path == null) yield break;

        if (drawPath)
        {
            GameManager.Instance.GetSystem<PathDrawer>().DrawPath(path);
        }

        _spriteRoot.gameObject.SetActive(true);
        _shadowRenderer.enabled = true;

        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        int index = 0;
        while (index < path.Length)
        {
            var speed = GameManager.Instance.GetSystem<TimeSystem>().TimeScale * 0.6f;
            if (gridmap.GetConstructionAt(path.Nodes[index].Position) == null)
            {
                speed *= 0.5f;
            }

            var worldPos = gridmap.CellToWorld(path.Nodes[index].Position);
            while (Vector2.Distance(transform.position, worldPos) > 0.1f)
            {
                var dir = (worldPos - (Vector2)transform.position).normalized;
                var velocity = speed * Time.deltaTime * dir;

                _spriteRoot.localScale = new Vector3(velocity.x < 0 ? 0.5f : -0.5f, 0.5f, 1);

                transform.position += (Vector3)velocity;

                _animator.SetFloat("RunState", 0.5f);
                yield return null;
            }
            index++;
            path.Location = index;
        }

        if (index == path.Length)
        {
            var construction = gridmap.GetConstructionAt(target);
            if (construction != null && construction.GetComponent<Road>() == null)
            {
                construction.VisitedHunters.Add(this);
                _spriteRoot.gameObject.SetActive(false);
                _shadowRenderer.enabled = false;
            }
        }

        if (drawPath)
        {
            GameManager.Instance.GetSystem<PathDrawer>().RemovePath(path);
        }

        _animator.SetFloat("RunState", 0);
    }

    private IEnumerator MoveTargetRoutine()
    {
        Construction clickedConstruction = null;

        while (clickedConstruction == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var colliders = Physics2D.OverlapPointAll(position);

                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent<Construction>(out var construction))
                    {
                        clickedConstruction = construction;
                        break;
                    }
                }
            }

            yield return null;
        }

        StartCoroutine(MoveRoutine(clickedConstruction.CellPos, true));
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
