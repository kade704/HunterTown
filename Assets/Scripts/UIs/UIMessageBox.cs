using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFade))]
public class UIMessageBox : MonoBehaviour
{
    private UIFade _fade;
    private Image _bg;
    private Image _icon;
    private Text _msg;

    public Image Icon => _icon;
    public Text Msg => _msg;
    public Color BgColor
    {
        set => _bg.color = value;
    }

    private void Awake()
    {
        _fade = GetComponent<UIFade>();
        _bg = GetComponent<Image>();
        _icon = transform.Find("Icon").GetComponent<Image>();
        _msg = transform.Find("Msg").GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(LifeTimeRoutine());
    }

    private IEnumerator LifeTimeRoutine()
    {
        yield return new WaitForSeconds(5f);
        _fade.FadeOut();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
