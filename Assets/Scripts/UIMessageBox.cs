using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageBox : MonoBehaviour
{
    private Image _icon;
    private Text _msg;

    public Image Icon => _icon;
    public Text Msg => _msg;

    private void Awake()
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _msg = transform.Find("Msg").GetComponent<Text>();
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
