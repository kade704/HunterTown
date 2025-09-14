using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    private CanvasGroup _mainButtonsCanvasGroup;
    private Button _startButton;
    private Button _optionsButton;
    private Button _exitGameButton;


    private CanvasGroup _loadGameButtonsCanvasGroup;
    private Button[] _loadGameButtons = new Button[3];
    private Button _backButton;

    private void Awake()
    {
        _mainButtonsCanvasGroup = transform.Find("MainButtons").GetComponent<CanvasGroup>();
        _startButton = transform.Find("MainButtons/StartButton").GetComponent<Button>();
        _optionsButton = transform.Find("MainButtons/OptionsButton").GetComponent<Button>();
        _exitGameButton = transform.Find("MainButtons/ExitGameButton").GetComponent<Button>();

        _loadGameButtonsCanvasGroup = transform.Find("LoadGameButtons").GetComponent<CanvasGroup>();
        _loadGameButtons[0] = transform.Find("LoadGameButtons/LoadGameButton1").GetComponent<Button>();
        _loadGameButtons[1] = transform.Find("LoadGameButtons/LoadGameButton2").GetComponent<Button>();
        _loadGameButtons[2] = transform.Find("LoadGameButtons/LoadGameButton3").GetComponent<Button>();
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
            SceneManager.LoadScene("Options");
        });
        _exitGameButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _backButton.onClick.AddListener(() =>
        {
            UIUtil.ShowCanvasGroup(_mainButtonsCanvasGroup);
            UIUtil.HideCanvasGroup(_loadGameButtonsCanvasGroup);
        });

        for (int i = 1; i <= 3; i++)
        {
            var saveIdx = i;

            var hasSaveFile = GameManager.Instance.GetSystem<SaveFileSystem>().HasSaveFile(saveIdx);

            var saveStr = GameManager.Instance.GetSystem<SaveFileSystem>().GetSaveTimeString(saveIdx) ?? "비어있음";
            _loadGameButtons[saveIdx - 1].GetComponentInChildren<Text>().text = $"세이브{saveIdx} [{saveStr}]";
            _loadGameButtons[saveIdx - 1].onClick.AddListener(() =>
            {
                GameManager.Instance.GetSystem<SaveFileSystem>().SetSelectedSave(saveIdx);
                if (hasSaveFile)
                {
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    SceneManager.LoadScene("Intro");
                }
            });
        }
    }
}
