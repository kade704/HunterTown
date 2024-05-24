using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    private CanvasGroup _mainButtonsCanvasGroup;
    private Button _startButton;
    private Button _optionsButton;
    private Button _exitGameButton;


    private CanvasGroup _loadGameButtonsCanvasGroup;
    private Button _loadGameButton1;
    private Button _loadGameButton2;
    private Button _loadGameButton3;
    private Button _backButton;

    private void Awake()
    {
        _mainButtonsCanvasGroup = transform.Find("MainButtons").GetComponent<CanvasGroup>();
        _startButton = transform.Find("MainButtons/StartButton").GetComponent<Button>();
        _optionsButton = transform.Find("MainButtons/OptionsButton").GetComponent<Button>();
        _exitGameButton = transform.Find("MainButtons/ExitGameButton").GetComponent<Button>();

        _loadGameButtonsCanvasGroup = transform.Find("LoadGameButtons").GetComponent<CanvasGroup>();
        _loadGameButton1 = transform.Find("LoadGameButtons/LoadGameButton1").GetComponent<Button>();
        _loadGameButton2 = transform.Find("LoadGameButtons/LoadGameButton2").GetComponent<Button>();
        _loadGameButton3 = transform.Find("LoadGameButtons/LoadGameButton3").GetComponent<Button>();
        _backButton = transform.Find("LoadGameButtons/BackButton").GetComponent<Button>();
    }

    private void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            UIUtil.HideCanvasGroup(_mainButtonsCanvasGroup);
            UIUtil.ShowCanvasGroup(_loadGameButtonsCanvasGroup);
        });
        _optionsButton.onClick.AddListener(() =>
        {
            GameManager.Instance.GoOptions();
        });
        _exitGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitGame();
        });

        _backButton.onClick.AddListener(() =>
        {
            UIUtil.ShowCanvasGroup(_mainButtonsCanvasGroup);
            UIUtil.HideCanvasGroup(_loadGameButtonsCanvasGroup);
        });

        var savePath1 = Application.persistentDataPath + $"/SaveData1.json";
        var saveStr1 = File.Exists(savePath1) ? File.GetLastWriteTime(savePath1).ToString() : "비어있음";
        _loadGameButton1.GetComponentInChildren<Text>().text = $"세이브1 [{saveStr1}]";
        _loadGameButton1.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGame(1);
        });

        var savePath2 = Application.persistentDataPath + $"/SaveData2.json";
        var saveStr2 = File.Exists(savePath2) ? File.GetLastWriteTime(savePath2).ToString() : "비어있음";
        _loadGameButton2.GetComponentInChildren<Text>().text = $"세이브2 [{saveStr2}]";
        _loadGameButton2.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGame(2);
        });

        var savePath3 = Application.persistentDataPath + $"/SaveData3.json";
        var saveStr3 = File.Exists(savePath3) ? File.GetLastWriteTime(savePath3).ToString() : "비어있음";
        _loadGameButton3.GetComponentInChildren<Text>().text = $"세이브3 [{saveStr3}]";
        _loadGameButton3.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGame(3);
        });
    }
}
