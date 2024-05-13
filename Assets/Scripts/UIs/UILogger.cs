using UnityEngine;

public class UILogger : MonoBehaviour
{
    [SerializeField] private UIMessageBox _messageBoxPrefab;
    [SerializeField] private Sprite _infoIcon;
    [SerializeField] private Sprite _warningIcon;
    [SerializeField] private Sprite _errorIcon;

    private void Awake()
    {
        GameManager.Instance.GetSystem<LoggerSystem>().OnLog.AddListener((message) =>
        {
            Log(message);
        });
    }


    private void Log(LoggerSystem.Message message)
    {
        var messageBox = Instantiate(_messageBoxPrefab, transform);
        messageBox.Icon.sprite = GetLogIconSprite(message.type);
        messageBox.Msg.text = message.content;

        messageBox.BgColor = GetLogColor(message.type);
    }

    private Sprite GetLogIconSprite(LoggerSystem.LogType type)
    {
        return type switch
        {
            LoggerSystem.LogType.Info => _infoIcon,
            LoggerSystem.LogType.Warning => _warningIcon,
            LoggerSystem.LogType.Error => _errorIcon,
            _ => null,
        };
    }

    private Color GetLogColor(LoggerSystem.LogType type)
    {
        return type switch
        {
            LoggerSystem.LogType.Info => new Color(0, 1, 0.5f),
            LoggerSystem.LogType.Warning => new Color(1, 1, 0.5f),
            LoggerSystem.LogType.Error => new Color(1, 0.5f, 0.5f),
            _ => Color.white,
        };
    }
}
