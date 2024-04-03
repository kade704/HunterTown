using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogger : Singleton<UILogger>
{
    private Transform _messageBoxParent;
    private UIMessageBox _messageBoxRef;

    protected override void Awake()
    {
        base.Awake();

        _messageBoxParent = transform.Find("MessageBoxes");
        _messageBoxRef = transform.Find("MessageBoxRef").GetComponent<UIMessageBox>();
    }


    public void LogInfo(string message)
    {
        var messageBox = Instantiate(_messageBoxRef, _messageBoxParent);
        messageBox.gameObject.SetActive(true);
        messageBox.Icon.sprite = Resources.Load<Sprite>("Icons/Info");
        messageBox.Msg.text = message;
    }

    public void LogWarning(string message)
    {
        var messageBox = Instantiate(_messageBoxRef, _messageBoxParent);
        messageBox.gameObject.SetActive(true);
        messageBox.Icon.sprite = Resources.Load<Sprite>("Icons/Warning");
        messageBox.Msg.text = message;
    }

    public void LogError(string message)
    {
        var messageBox = Instantiate(_messageBoxRef, _messageBoxParent);
        messageBox.gameObject.SetActive(true);
        messageBox.Icon.sprite = Resources.Load<Sprite>("Icons/Error");
        messageBox.Msg.text = message;
    }
}
