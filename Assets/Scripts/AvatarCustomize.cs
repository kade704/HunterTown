using UnityEngine;

public class AvatarCustomize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bodyClothRenderer;
    [SerializeField] private SpriteRenderer _hairRenderer;
    [SerializeField] private SpriteRenderer _leftSleeveRenderer;
    [SerializeField] private SpriteRenderer _rightSleeveRenderer;
    [SerializeField] private SpriteRenderer _leftPantRenderer;
    [SerializeField] private SpriteRenderer _rightPantRenderer;
    [SerializeField] private Transform _spriteRoot;
    [SerializeField] private SpriteRenderer _shadowRenderer;

    private TopCloth _topCloth;
    private BottomCloth _bottomCloth;
    private Hair _hair;
    private Color _hairColor;

    public TopCloth TopCloth
    {
        get => _topCloth;
        set
        {
            _topCloth = value;
            _bodyClothRenderer.sprite = value.body;
            _leftSleeveRenderer.sprite = value.armL;
            _rightSleeveRenderer.sprite = value.armR;
        }
    }

    public BottomCloth BottomCloth
    {
        get => _bottomCloth;
        set
        {
            _bottomCloth = value;
            _leftPantRenderer.sprite = value.legL;
            _rightPantRenderer.sprite = value.legR;
        }
    }

    public Hair Hair
    {
        get => _hair;
        set
        {
            _hair = value;
            _hairRenderer.sprite = value.hair;
        }
    }

    public Color HairColor
    {
        get => _hairColor;
        set
        {
            _hairColor = value;
            _hairRenderer.color = value;
        }
    }
    public bool IsVisible { get { return _spriteRoot.gameObject.activeSelf; } }

    public void ShowAvatar()
    {
        _spriteRoot.gameObject.SetActive(true);
        _shadowRenderer.gameObject.SetActive(true);
    }

    public void HideAvatar()
    {
        _spriteRoot.gameObject.SetActive(false);
        _shadowRenderer.gameObject.SetActive(false);
    }

    public void CopyAvatar(AvatarCustomize other)
    {
        TopCloth = other.TopCloth;
        BottomCloth = other.BottomCloth;
        Hair = other.Hair;
        HairColor = other.HairColor;
    }
}
