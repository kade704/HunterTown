using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitGameButton;

    private void Start()
    {
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
            GameManager.Instance.GoOptions();
        });
        _exitGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitGame();
        });
    }
}
