using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Construction : MonoBehaviour {
    public enum Type { ROAD, BUILDING }
    public enum Direction { SOUTH, WEST, NORTH, EAST }

    [SerializeField] private Type _type;          //건물의 타입
    [SerializeField] private RuleTile _roadTile;  //길의 타일정보
    [SerializeField] private int _cost;           //건물 설치 가격
    [SerializeField] private string _displayName; //UI에 표시되는 이름
    [SerializeField] private Sprite _icon;        //UI에 표시되는 아이콘

    [Header("방향별 스프라이트")]
    [SerializeField] private Sprite _eastSprite;
    [SerializeField] private Sprite _southSprite;
    [SerializeField] private Sprite _westSprite;
    [SerializeField] private Sprite _northSprite;

    private SpriteRenderer _spriteRenderer;
    private Vector2Int _cellPos;
    private Direction _direction;

    public Vector2Int CellPos { get { return _cellPos; } set { _cellPos = value; } }
    public Direction Direction_ {
        get {
            return _direction;
        }
        set {
            _direction = value;
            _spriteRenderer.sprite = GetSpriteFromDirection(value);
        }
    }

    public Type ConstructionType => _type;
    public int Cost => _cost;
    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public RuleTile RoadTile => _roadTile;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Sprite GetSpriteFromDirection(Direction direction) {
        switch (direction) {
            case Direction.EAST:
                return _eastSprite;
            case Direction.WEST:
                return _westSprite;
            case Direction.SOUTH:
                return _southSprite;
            case Direction.NORTH:
                return _northSprite;
            default:
                break;
        }
        return null;
    }
}
