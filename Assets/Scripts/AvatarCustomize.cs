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

    public Sprite BodyClothSprite { get { return _bodyClothRenderer.sprite; } set { _bodyClothRenderer.sprite = value; } }
    public Sprite HairSprite { get { return _hairRenderer.sprite; } set { _hairRenderer.sprite = value; } }
    public Sprite LeftSleeveSprite { get { return _leftSleeveRenderer.sprite; } set { _leftSleeveRenderer.sprite = value; } }
    public Sprite RightSleeveSprite { get { return _rightSleeveRenderer.sprite; } set { _rightSleeveRenderer.sprite = value; } }
    public Sprite LeftPantSprite { get { return _leftPantRenderer.sprite; } set { _leftPantRenderer.sprite = value; } }
    public Sprite RightPantSprite { get { return _rightPantRenderer.sprite; } set { _rightPantRenderer.sprite = value; } }
    public Color HairColor { get { return _hairRenderer.color; } set { _hairRenderer.color = value; } }

    public void ShowAvatar(bool show)
    {
        _spriteRoot.gameObject.SetActive(show);
        _shadowRenderer.gameObject.SetActive(show);
    }

}
