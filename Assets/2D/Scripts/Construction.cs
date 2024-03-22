using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Construction : MonoBehaviour {
    public enum Type { ROAD, BUILDING }

    [SerializeField] private Type _type;          //건물의 타입
    [SerializeField] private int _cost;           //건물 설치 가격
    [SerializeField] private string _displayName; //UI에 표시되는 이름
    [SerializeField] private Sprite _icon;        //UI에 표시되는 아이콘

    private SpriteRenderer _spriteRenderer;
    private Vector2Int _cellPos;

    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }

    public Type StructureType => _type;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
