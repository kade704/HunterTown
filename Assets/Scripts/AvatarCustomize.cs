using UnityEngine;

public class AvatarCustomize : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _headBaseRenderer;
    [SerializeField] private SpriteRenderer _bodyBaseRenderer;
    [SerializeField] private SpriteRenderer _armLeftBaseRenderer;
    [SerializeField] private SpriteRenderer _armRightBaseRenderer;
    [SerializeField] private SpriteRenderer _legLeftBaseRenderer;
    [SerializeField] private SpriteRenderer _legRightBaseRenderer;
    [SerializeField] private SpriteRenderer _hairRenderer;
    [SerializeField] private SpriteRenderer _bodyClothRenderer;
    [SerializeField] private SpriteRenderer _armLeftClothRenderer;
    [SerializeField] private SpriteRenderer _armRightClothRenderer;
    [SerializeField] private SpriteRenderer _legLeftClothRenderer;
    [SerializeField] private SpriteRenderer _legRightClothRenderer;
    [SerializeField] private SpriteRenderer _bodyArmorRenderer;
    [SerializeField] private SpriteRenderer _armLeftArmorRenderer;
    [SerializeField] private SpriteRenderer _armRightArmorRenderer;
    [SerializeField] private SpriteRenderer _helmetRenderer;
    [SerializeField] private SpriteRenderer _weaponRenderer;
    [SerializeField] private SpriteRenderer _eyeLeftRenderer;
    [SerializeField] private SpriteRenderer _eyeRightRenderer;
    [SerializeField] private Transform _spriteRoot;
    [SerializeField] private SpriteRenderer _shadowRenderer;

    private BaseBody _baseBody;
    private TopCloth _topCloth;
    private BottomCloth _bottomCloth;
    private Armor _armor;
    private Helmet _helmet;
    private Hair _hair;
    private Eye _eye;
    private Color _hairColor;
    private Color _eyeColor;
    private Weapon _weapon;

    public BaseBody BaseBody
    {
        get => _baseBody;
        set
        {
            _baseBody = value;
            _headBaseRenderer.sprite = value.head;
            _bodyBaseRenderer.sprite = value.body;
            _armLeftBaseRenderer.sprite = value.armL;
            _armRightBaseRenderer.sprite = value.armR;
            _legLeftBaseRenderer.sprite = value.legL;
            _legRightBaseRenderer.sprite = value.legR;
        }
    }

    public TopCloth TopCloth
    {
        get => _topCloth;
        set
        {
            _topCloth = value;
            _bodyClothRenderer.sprite = value.body;
            _armLeftClothRenderer.sprite = value.armL;
            _armRightClothRenderer.sprite = value.armR;
        }
    }

    public BottomCloth BottomCloth
    {
        get => _bottomCloth;
        set
        {
            _bottomCloth = value;
            _legLeftClothRenderer.sprite = value.legL;
            _legRightClothRenderer.sprite = value.legR;
        }
    }

    public Armor Armor
    {
        get => _armor;
        set
        {
            _armor = value;
            _bodyArmorRenderer.sprite = value.body;
            _armLeftArmorRenderer.sprite = value.armL;
            _armRightArmorRenderer.sprite = value.armR;
        }
    }

    public Helmet Helmet
    {
        get => _helmet;
        set
        {
            _helmet = value;
            _helmetRenderer.sprite = value.hair;
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

    public Eye Eye
    {
        get => _eye;
        set
        {
            _eye = value;
            _eyeLeftRenderer.sprite = value.eye;
            _eyeRightRenderer.sprite = value.eye;
        }
    }

    public Color EyeColor
    {
        get => _eyeColor;
        set
        {
            _eyeColor = value;
            _eyeLeftRenderer.color = value;
            _eyeRightRenderer.color = value;
        }
    }

    public Weapon Weapon
    {
        get => _weapon;
        set
        {
            _weapon = value;
            _weaponRenderer.sprite = value.weapon;
        }
    }

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
        BaseBody = other.BaseBody;
        TopCloth = other.TopCloth;
        BottomCloth = other.BottomCloth;
        Hair = other.Hair;
        HairColor = other.HairColor;
        EyeColor = other.EyeColor;
        Eye = other.Eye;
        Armor = other.Armor;
        Helmet = other.Helmet;
        Weapon = other.Weapon;
    }
}
