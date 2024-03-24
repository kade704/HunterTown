using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Construction : MonoBehaviour {
    [SerializeField] private int _cost;           //건물 설치 가격
    [SerializeField] private string _displayName; //UI에 표시되는 이름
    [SerializeField] private Sprite _icon;        //UI에 표시되는 아이콘

    protected SpriteRenderer _spriteRenderer;
    protected Vector2Int _cellPos;
    protected ConstructionMap _constructionMap;

    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }
    public ConstructionMap ConstructionMap { get { return _constructionMap; } set { _constructionMap = value; } }

    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;

    protected virtual void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _constructionMap = GetComponentInParent<ConstructionMap>();
    }

    protected virtual void Update() {
        _spriteRenderer.sortingOrder = 500 - Mathf.FloorToInt(transform.position.y);
    }

    public void DestroyThis() {
        _constructionMap.RemoveConstruction(CellPos);
    }

    public void SetOutline(bool outline) {
        _spriteRenderer.material.SetFloat("_Outline", outline ? 1 : 0);
    }
}
