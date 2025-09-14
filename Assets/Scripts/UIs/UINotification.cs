using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINotification : MonoBehaviour
{
    [SerializeField] private UINotificationMessage _messagePrefab;
    [SerializeField] private Sprite _infoIcon;
    [SerializeField] private Sprite _warningIcon;
    [SerializeField] private Sprite _errorIcon;

    private Dictionary<NotificationSystem.Message, UINotificationMessage> _messageMap = new();

    private void Start()
    {
        GameManager.Instance.GetSystem<NotificationSystem>().OnMessageChanged.AddListener(OnMessageChanged);
        OnMessageChanged();
    }

    private void OnMessageChanged()
    {
        var messageBoxes = _messageMap.Values.ToList();

        foreach (var message in GameManager.Instance.GetSystem<NotificationSystem>().Messages)
        {
            if (!_messageMap.ContainsKey(message))
            {
                var messageBox = Instantiate(_messagePrefab, transform);
                messageBox.Icon.sprite = GetLogIconSprite(message.type);
                messageBox.ContentText.text = message.content;
                messageBox.BgColor = GetLogColor(message.type);
                _messageMap.Add(message, messageBox);
            }
            else
            {
                messageBoxes.Remove(_messageMap[message]);
            }
        }

        foreach (var messageBox in messageBoxes)
        {
            _messageMap.Remove(_messageMap.First(x => x.Value == messageBox).Key);
            Destroy(messageBox.gameObject);
        }
    }

    private Sprite GetLogIconSprite(NotificationSystem.NotificationType type)
    {
        return type switch
        {
            NotificationSystem.NotificationType.Info => _infoIcon,
            NotificationSystem.NotificationType.Warning => _warningIcon,
            NotificationSystem.NotificationType.Error => _errorIcon,
            _ => null,
        };
    }

    private Color GetLogColor(NotificationSystem.NotificationType type)
    {
        return type switch
        {
            NotificationSystem.NotificationType.Info => new Color(0, 1, 0.5f),
            NotificationSystem.NotificationType.Warning => new Color(1, 1, 0.5f),
            NotificationSystem.NotificationType.Error => new Color(1, 0.5f, 0.5f),
            _ => Color.white,
        };
    }
}
