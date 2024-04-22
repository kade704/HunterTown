using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoggerSystem : MonoBehaviour
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }
    public struct Message
    {
        public string content;
        public LogType type;
    }

    private List<Message> _messages = new();
    private UnityEvent<Message> _onLog = new();

    public List<Message> Messages => _messages;
    public UnityEvent<Message> OnLog => _onLog;


    public void Log(LogType type, string content)
    {
        var message = new Message { type = type, content = content };
        _messages.Add(message);
        _onLog.Invoke(message);
        Debug.Log($"InGame Log: [{type}] {content}");
    }

    public void LogError(string content)
    {
        Log(LogType.Error, content);
    }

    public void LogWarning(string content)
    {
        Log(LogType.Warning, content);
    }

    public void LogInfo(string content)
    {
        Log(LogType.Info, content);
    }
}
