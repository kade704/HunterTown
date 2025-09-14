using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Hunter : MonoBehaviour
{
    [ReadOnly][SerializeField] private float _defaultHp;
    [ReadOnly][SerializeField] private float _defaultDamage;

    private Interactable _interactable;
    private Visitable _lastVisited;

    private Animator _animator;

    private Sprite _thumbnail;
    private AvatarMovement _avatarMovement;
    private AvatarCustomize _avatarCustomize;
    private Coroutine _moveTargetRoutine;
    private Path _movePath;

    public float DefaultHp
    {
        get => _defaultHp;
        set => _defaultHp = value;
    }

    public float DefaultDamage
    {
        get => _defaultDamage;
        set => _defaultDamage = value;
    }

    public Sprite Thumbnail
    {
        get => _thumbnail;
    }

    public AvatarCustomize AvatarCustomize => _avatarCustomize;
    public Interactable Interactable => _interactable;


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
        _avatarMovement = GetComponent<AvatarMovement>();
        _avatarCustomize = GetComponent<AvatarCustomize>();
    }

    private void Start()
    {
        _interactable.OnSelected.AddListener(OnSelected);
        _interactable.OnDeselected.AddListener(OnDeselected);
        _interactable.OnInteracted.AddListener(OnInteracted);

        StartCoroutine(CaptureThumbnailRoutine());
    }

    private void Update()
    {
        _interactable.SubDescription = $"체력: {_defaultHp}\n공격력: {_defaultDamage}";
    }

    private void OnSelected()
    {
        if (_movePath != null)
        {
            GameManager.Instance.GetSystem<PathDrawer>().DrawPath(_movePath);
        }
    }

    private void OnDeselected()
    {
        if (_movePath != null)
        {
            GameManager.Instance.GetSystem<PathDrawer>().RemovePath();
        }
    }

    private void OnInteracted(Interaction interaction)
    {
        if (interaction.ID == "#move_target")
        {
            _animator.SetFloat("RunState", 0);
            if (_moveTargetRoutine != null)
            {
                StopCoroutine(_moveTargetRoutine);
                GameManager.Instance.GetSystem<PathDrawer>().RemovePath();
            }
            _moveTargetRoutine = StartCoroutine(MoveTargetRoutine());
        }
    }

    private IEnumerator MoveTargetRoutine()
    {
        GameManager.Instance.GetSystem<InteractableSelector>().EnableSelect = false;

        Visitable clickedVisitable = null;
        while (clickedVisitable == null)
        {
            if (Input.GetMouseButtonDown(0) && !UIUtil.IsUIObjectOverPointer())
            {
                clickedVisitable = GetVisitableOverPointer();
            }

            yield return null;
        }

        GameManager.Instance.GetSystem<InteractableSelector>().EnableSelect = true;

        if (_lastVisited)
        {
            _lastVisited.ExitVisitor(this);
            _lastVisited = null;
        }

        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        var start = gridmap.WorldToCell(transform.position);
        _movePath = GameManager.Instance.GetSystem<PathFinder>().SearchPath(start, clickedVisitable.Construction.CellPos);

        if (_movePath == null)
        {
            GameManager.Instance.GetSystem<NotificationSystem>().NotifyWarning("경로를 찾을 수 없습니다.");
            yield break;
        }

        GameManager.Instance.GetSystem<PathDrawer>().DrawPath(_movePath);

        yield return _avatarMovement.MoveRoutine(_movePath);

        if (clickedVisitable && clickedVisitable.GetComponent<Visitable>() != null)
        {
            _lastVisited = clickedVisitable.GetComponent<Visitable>();
            _lastVisited.EnterVisitor(this);
        }

        if (_movePath == GameManager.Instance.GetSystem<PathDrawer>().Path)
        {
            GameManager.Instance.GetSystem<PathDrawer>().RemovePath();
        }
        _movePath = null;
    }

    private Visitable GetVisitableOverPointer()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var colliders = Physics2D.OverlapPointAll(position);

        Visitable visitable = null;
        foreach (var collider in colliders)
        {
            visitable = collider.GetComponent<Visitable>();
            if (visitable) break;
        }
        return visitable;
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
