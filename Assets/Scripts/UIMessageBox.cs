using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageBox : UIPanel
{
    private Image _bg;
    private Image _icon;
    private Text _msg;

    public Image Icon => _icon;
    public Text Msg => _msg;
    public Color BgColor
    {
        set => _bg.color = value;
    }

    protected override void Awake()
    {
        base.Awake();

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
        HidePanel();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
