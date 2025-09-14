using UnityEngine;

public class AvatarCustomize : MonoBehaviour
{
    private SpriteRenderer _headBaseRenderer;
    private SpriteRenderer _bodyBaseRenderer;
    private SpriteRenderer _armLeftBaseRenderer;
    private SpriteRenderer _armRightBaseRenderer;
    private SpriteRenderer _legLeftBaseRenderer;
    private SpriteRenderer _legRightBaseRenderer;
    private SpriteRenderer _hairRenderer;
    private SpriteRenderer _bodyClothRenderer;
    private SpriteRenderer _armLeftClothRenderer;
    private SpriteRenderer _armRightClothRenderer;
    private SpriteRenderer _legLeftClothRenderer;
    private SpriteRenderer _legRightClothRenderer;
    private SpriteRenderer _bodyArmorRenderer;
    private SpriteRenderer _armLeftArmorRenderer;
    private SpriteRenderer _armRightArmorRenderer;
    private SpriteRenderer _helmetRenderer;
    private SpriteRenderer _weaponRenderer;
    private SpriteRenderer _eyeLeftRenderer;
    private SpriteRenderer _eyeRightRenderer;
    private Transform _spriteRoot;
    private SpriteRenderer _shadowRenderer;
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

    private void Awake()
    {
        _headBaseRenderer = transform.Find("Root/Head/Base").GetComponent<SpriteRenderer>();
        _bodyBaseRenderer = transform.Find("Root/Body/Base").GetComponent<SpriteRenderer>();
        _armLeftBaseRenderer = transform.Find("Root/LeftArm/Base").GetComponent<SpriteRenderer>();
        _armRightBaseRenderer = transform.Find("Root/RightArm/Base").GetComponent<SpriteRenderer>();
        _legLeftBaseRenderer = transform.Find("Root/LeftFoot/Base").GetComponent<SpriteRenderer>();
        _legRightBaseRenderer = transform.Find("Root/RightFoot/Base").GetComponent<SpriteRenderer>();
        _hairRenderer = transform.Find("Root/Head/Hair").GetComponent<SpriteRenderer>();
        _bodyClothRenderer = transform.Find("Root/Body/Cloth").GetComponent<SpriteRenderer>();
        _armLeftClothRenderer = transform.Find("Root/LeftArm/Cloth").GetComponent<SpriteRenderer>();
        _armRightClothRenderer = transform.Find("Root/RightArm/Cloth").GetComponent<SpriteRenderer>();
        _legLeftClothRenderer = transform.Find("Root/LeftFoot/Cloth").GetComponent<SpriteRenderer>();
        _legRightClothRenderer = transform.Find("Root/RightFoot/Cloth").GetComponent<SpriteRenderer>();
        _bodyArmorRenderer = transform.Find("Root/Body/Armor").GetComponent<SpriteRenderer>();
        _armLeftArmorRenderer = transform.Find("Root/LeftArm/Armor").GetComponent<SpriteRenderer>();
        _armRightArmorRenderer = transform.Find("Root/RightArm/Armor").GetComponent<SpriteRenderer>();
        _helmetRenderer = transform.Find("Root/Head/Helmet").GetComponent<SpriteRenderer>();
        _weaponRenderer = transform.Find("Root/LeftArm/Weapon").GetComponent<SpriteRenderer>();
        _eyeLeftRenderer = transform.Find("Root/Head/Eyes/LeftEye").GetComponent<SpriteRenderer>();
        _eyeRightRenderer = transform.Find("Root/Head/Eyes/RightEye").GetComponent<SpriteRenderer>();
        _spriteRoot = transform.Find("Root");
        _shadowRenderer = transform.Find("Shadow/Shadow").GetComponent<SpriteRenderer>();
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
