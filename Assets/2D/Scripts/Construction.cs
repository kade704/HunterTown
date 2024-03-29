using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Construction : MonoBehaviour
{
    [SerializeField] private int _cost;           //�ǹ� ��ġ ����
    [SerializeField] private string _displayName; //UI�� ǥ�õǴ� �̸�
    [SerializeField] private Sprite _icon;        //UI�� ǥ�õǴ� ������

    protected UnityEvent _onClicked = new();
    protected SpriteRenderer _spriteRenderer;
    protected Vector2Int _cellPos;
    protected ConstructionMap _constructionMap;
    protected int _defaultOrder;

    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }
    public UnityEvent OnClicked => _onClicked;

    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public Sprite CurrentSprite => _spriteRenderer.sprite;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _constructionMap = GetComponentInParent<ConstructionMap>();
        _defaultOrder = _spriteRenderer.sortingOrder;
    }

    protected virtual void Update()
    {
        _spriteRenderer.sortingOrder = _defaultOrder - Mathf.FloorToInt(transform.position.y * 10);
    }

    private void OnDestroy()
    {
        if (_constructionMap.GetConstruction(CellPos) == this)
            _constructionMap.RemoveConstruction(CellPos);
    }

    public void SetOutline(bool outline)
    {
        _spriteRenderer.material.SetFloat("_Outline", outline ? 1 : 0);
    }
}
