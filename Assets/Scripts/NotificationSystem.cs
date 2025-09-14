using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationSystem : MonoBehaviour
{
    public enum NotificationType
    {
        Info,
        Warning,
        Error
    }

    public class Message
    {
        public string content;
        public NotificationType type;
        public bool once;
    }

    private List<Message> _messages = new();
    private UnityEvent _onMessageChanged = new();

    public List<Message> Messages => _messages;
    public UnityEvent OnMessageChanged => _onMessageChanged;


    public Message Notify(NotificationType type, string content, bool once = true)
    {
        var message = new Message { type = type, content = content, once = once };
        _messages.Add(message);
        _onMessageChanged.Invoke();

        if (once)
        {
            StartCoroutine(RemoveMessageOnceRoutine(message));
        }

        Debug.Log($"NotificationSystem: [{type}] {content}");
        return message;
    }

    public void RemoveMessage(Message message)
    {
        _messages.Remove(message);
        _onMessageChanged.Invoke();
    }

    private IEnumerator RemoveMessageOnceRoutine(Message message)
    {
        yield return new WaitForSeconds(5);
        RemoveMessage(message);
    }

    public Message NotifyError(string content, bool once = true)
    {
        return Notify(NotificationType.Error, content, once);
    }

    public Message NotifyWarning(string content, bool once = true)
    {
        return Notify(NotificationType.Warning, content, once);
    }

    public Message NotifyInfo(string content, bool once = true)
    {
        return Notify(NotificationType.Info, content, once);
    }
}
