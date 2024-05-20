using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Interactable))]
public class Hunter : MonoBehaviour
{
    [ReadOnly][SerializeField] private string _displayName;
    [ReadOnly][SerializeField] private float _defaultHp;
    [ReadOnly][SerializeField] private float _defaultDamage;

    private Interactable _interactable;
    private Construction _visitedConstruction;

    private Animator _animator;
    private SortingGroup _sortingGroup;
    private Sprite _thumbnail;
    private AvatarMovement _avatarMovement;
    private AvatarCustomize _avatarCustomize;
    private Coroutine _moveTargetRoutine;
    private Path _movePath;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public float DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public float DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public Sprite Thumbnail { get { return _thumbnail; } set { _thumbnail = value; } }
    public AvatarCustomize AvatarCustomize { get { return _avatarCustomize; } }


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
        _avatarMovement = GetComponent<AvatarMovement>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
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
                if (_movePath != null)
                {
                    StopCoroutine(_moveTargetRoutine);
                    GameManager.Instance.GetSystem<PathDrawer>().RemovePath(_movePath);
                }
                _moveTargetRoutine = StartCoroutine(MoveTargetRoutine());
            }
        });

        StartCoroutine(CaptureThumbnailRoutine());
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10) + 1;
    }


    private IEnumerator MoveTargetRoutine()
    {
        Construction clickedConstruction = null;

        while (clickedConstruction == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickedConstruction = GetClickedConstruction();
                if (clickedConstruction != null && clickedConstruction.Visitable)
                {
                    break;
                }
            }

            yield return null;
        }

        if (_visitedConstruction)
        {
            _visitedConstruction.ExitVisitor(this);
            _visitedConstruction = null;
        }

        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        var start = gridmap.WorldToCell(transform.position);
        _movePath = GameManager.Instance.GetSystem<PathFinder>().SearchPath(start, clickedConstruction.CellPos);

        if (_movePath == null)
        {
            GameManager.Instance.GetSystem<LoggerSystem>().LogWarning("경로를 찾을 수 없습니다.");
            yield break;
        }

        GameManager.Instance.GetSystem<PathDrawer>().DrawPath(_movePath);

        yield return _avatarMovement.MoveRoutine(_movePath);

        if (clickedConstruction && clickedConstruction.Visitable)
        {
            _visitedConstruction = clickedConstruction;
            clickedConstruction.EnterVisitor(this);
        }

        GameManager.Instance.GetSystem<PathDrawer>().RemovePath(_movePath);
    }

    private Construction GetClickedConstruction()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var colliders = Physics2D.OverlapPointAll(position);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Construction>(out var construction))
            {
                return construction;
            }
        }

        return null;
    }

    public IEnumerator CaptureThumbnailRoutine(int resolution = 64)
    {
        var position = transform.position;
        transform.position = new Vector3(Random.Range(456, 789), Random.Range(456, 789), 0);

        yield return null;

        var renderTexture = RenderTexture.GetTemporary(resolution, resolution, 16, RenderTextureFormat.ARGB32);

        var camera = new GameObject("Camera").AddComponent<Camera>();
        camera.transform.parent = transform;
        camera.transform.localPosition = new Vector3(0, 0.3f, -10);
        camera.orthographic = true;
        camera.orthographicSize = 0.2f;
        camera.cullingMask = 1 << LayerMask.NameToLayer("Hunter");
        camera.targetTexture = renderTexture;
        camera.Render();

        Texture2D texture = new(resolution, resolution, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point
        };

        var previousRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        RenderTexture.ReleaseTemporary(renderTexture);
        camera.targetTexture = null;
        RenderTexture.active = previousRenderTexture;

        texture.Apply();

        Destroy(camera.gameObject);

        _thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 64);

        transform.position = position;
    }
}
