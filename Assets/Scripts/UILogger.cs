using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogger : Singleton<UILogger>
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    private Transform _messageBoxParent;
    private UIMessageBox _messageBoxRef;

    protected override void Awake()
    {
        base.Awake();

        _messageBoxParent = transform.Find("MessageBoxes");
        _messageBoxRef = transform.Find("MessageBoxRef").GetComponent<UIMessageBox>();
    }


    public void Log(LogType type, string message)
    {
        var messageBox = Instantiate(_messageBoxRef, _messageBoxParent);
        messageBox.transform.SetAsFirstSibling();
        messageBox.gameObject.SetActive(true);
        messageBox.Icon.sprite = GetLogIconSprite(type);
        messageBox.Msg.text = message;

        messageBox.BgColor = GetLogColor(type);
    }

    private Sprite GetLogIconSprite(LogType type)
    {
        string path = "";
        switch (type)
        {
            case LogType.Info:
                path = "Icons/Info";
                break;
            case LogType.Warning:
                path = "Icons/Warning";
                break;
            case LogType.Error:
                path = "Icons/Error";
                break;
        }

        return Resources.Load<Sprite>(path);
    }

    private Color GetLogColor(LogType type)
    {
        switch (type)
        {
            case LogType.Info:
                return new Color(0, 1, 0.5f);
            case LogType.Warning:
                return new Color(1, 1, 0.5f);
            case LogType.Error:
                return new Color(1, 0.5f, 0.5f);
        }

        return Color.white;
    }
}
