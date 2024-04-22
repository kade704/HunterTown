using UnityEngine;

public class UILogger : MonoBehaviour
{


    private Transform _messageBoxParent;
    private UIMessageBox _messageBoxRef;

    private void Awake()
    {
        _messageBoxParent = transform.Find("MessageBoxes");
        _messageBoxRef = transform.Find("MessageBoxRef").GetComponent<UIMessageBox>();

        GameManager.Instance.GetSystem<LoggerSystem>().OnLog.AddListener((message) =>
        {
            Log(message);
        });
    }


    private void Log(LoggerSystem.Message message)
    {
        var messageBox = Instantiate(_messageBoxRef, _messageBoxParent);
        messageBox.transform.SetAsFirstSibling();
        messageBox.gameObject.SetActive(true);
        messageBox.Icon.sprite = GetLogIconSprite(message.type);
        messageBox.Msg.text = message.content;

        messageBox.BgColor = GetLogColor(message.type);
    }

    private Sprite GetLogIconSprite(LoggerSystem.LogType type)
    {
        string path = "";
        switch (type)
        {
            case LoggerSystem.LogType.Info:
                path = "Icons/Info";
                break;
            case LoggerSystem.LogType.Warning:
                path = "Icons/Warning";
                break;
            case LoggerSystem.LogType.Error:
                path = "Icons/Error";
                break;
        }

        return Resources.Load<Sprite>(path);
    }

    private Color GetLogColor(LoggerSystem.LogType type)
    {
        switch (type)
        {
            case LoggerSystem.LogType.Info:
                return new Color(0, 1, 0.5f);
            case LoggerSystem.LogType.Warning:
                return new Color(1, 1, 0.5f);
            case LoggerSystem.LogType.Error:
                return new Color(1, 0.5f, 0.5f);
        }

        return Color.white;
    }
}
