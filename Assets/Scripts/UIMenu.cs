using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _optionsButton;
    private Button _exitGameButton;

    private void Awake()
    {
        _newGameButton = transform.Find("NewGameButton").GetComponent<Button>();
        _loadGameButton = transform.Find("LoadGameButton").GetComponent<Button>();
        _optionsButton = transform.Find("OptionsButton").GetComponent<Button>();
        _exitGameButton = transform.Find("ExitGameButton").GetComponent<Button>();

        _newGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
        });
        _loadGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.LoadGame();
        });
        _optionsButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ShowOptions();
        });
        _exitGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitGame();
        });
    }
}
