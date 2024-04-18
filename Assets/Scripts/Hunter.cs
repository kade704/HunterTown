using System.Collections;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

public class Hunter : MonoBehaviour
{
    [SerializeField] private string _displayName;
    [SerializeField] private float _defaultHp;
    [SerializeField] private float _defaultDamage;
    private bool _isDispatched = false;
    private ConstructionGridMap _constructionGridMap;
    private SpriteRenderer _clothSprite;
    private SpriteRenderer _hairSprite;
    private SpriteRenderer _leftSleeveSprite;
    private SpriteRenderer _rightSleeveSprite;
    private SortingGroup _sortingGroup;
    private Transform _spriteRoot;
    private Animator _animator;
    private Sprite _thumbnail;

    public string DisplayName { get { return _displayName; } set { _displayName = value; } }
    public float DefaultHp { get { return _defaultHp; } set { _defaultHp = value; } }

    public float DefaultDamage { get { return _defaultDamage; } set { _defaultDamage = value; } }

    public bool IsDispatched { get { return _isDispatched; } set { _isDispatched = value; } }
    public Sprite Thumbnail { get { return _thumbnail; } set { _thumbnail = value; } }
    public Sprite ClothSprite { set { _clothSprite.sprite = value; } }
    public Sprite HairSprite { set { _hairSprite.sprite = value; } }
    public Sprite LeftSleeveSprite { set { _leftSleeveSprite.sprite = value; } }
    public Sprite RightSleeveSprite { set { _rightSleeveSprite.sprite = value; } }
    public Color HairColor { set { _hairSprite.color = value; } }

    public bool Dispatch
    {
        set
        {
            _isDispatched = value;
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
        _animator = GetComponent<Animator>();
        _sortingGroup = GetComponent<SortingGroup>();
        _constructionGridMap = FindObjectOfType<ConstructionGridMap>();

        _spriteRoot = transform.Find("Root");
        _clothSprite = transform.Find("Root/BodySet/P_Body/Body/P_ClothBody/ClothBody").GetComponent<SpriteRenderer>();
        _hairSprite = transform.Find("Root/BodySet/P_Body/HeadSet/P_Head/P_Hair/7_Hair").GetComponent<SpriteRenderer>();
        _leftSleeveSprite = transform.Find("Root/BodySet/P_Body/ArmSet/ArmL/P_LArm/P_Arm/20_L_Arm/P_LCArm/21_LCArm").GetComponent<SpriteRenderer>();
        _rightSleeveSprite = transform.Find("Root/BodySet/P_Body/ArmSet/ArmR/P_RArm/P_Arm/-20_R_Arm/P_RCArm/-19_RCArm").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var _roads = _constructionGridMap.Constructions.FilterCast<Road>().ToArray();

        Road start = null;
        if (_roads.Length > 0)
        {
            start = _roads[Random.Range(0, _roads.Length)];
            transform.position = start.transform.position;
        }
        CaptureThumbnailRoutine();
        StartCoroutine(MoveRoutine(start));
        StartCoroutine(CaptureThumbnailRoutine());
    }

    private void Update()
    {
        _sortingGroup.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);
        _spriteRoot.gameObject.SetActive(!_isDispatched);
    }

    private IEnumerator MoveRoutine(Road start)
    {
        yield return new WaitForSeconds(Random.Range(3, 10));

        var _roads = _constructionGridMap.Constructions.FilterCast<Road>().ToArray();
        Road target = null;
        if (_roads.Length > 0)
        {
            target = _roads[Random.Range(0, _roads.Length)];

            var path = PathFinder.SearchPath(start, target, _roads);

            int index = 0;

            while (index < path.Length)
            {
                if (!path[index]) break;
                while (Vector3.Distance(transform.position, path[index].transform.position) > 0.01)
                {
                    var speed = Timer.Instance.TimeScale;
                    var velocity = (path[index].transform.position - transform.position).normalized * Time.deltaTime * speed;
                    _spriteRoot.localScale = new Vector3(velocity.x < 0 ? 0.5f : -0.5f, 0.5f, 1);
                    transform.position += velocity;
                    _animator.SetFloat("RunState", 0.5f);
                    yield return null;
                }
                index++;
            }
        }

        _animator.SetFloat("RunState", 0);

        StartCoroutine(MoveRoutine(target));
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
