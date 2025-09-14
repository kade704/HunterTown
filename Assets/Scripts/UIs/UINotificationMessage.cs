using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationMessage : MonoBehaviour
{
    private Image _backgroundImage;
    private Image _iconImage;
    private Text _contentText;

    public Image Icon => _iconImage;
    public Text ContentText => _contentText;
    public Color BgColor
    {
        set => _backgroundImage.color = value;
    }

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
        _iconImage = transform.Find("IconImage").GetComponent<Image>();
        _contentText = transform.Find("ContentText").GetComponent<Text>();
    }
}
