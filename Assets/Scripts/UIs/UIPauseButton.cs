using UnityEngine;
using UnityEngine.UI;

public class UIPauseButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            GameManager.Instance.GetSystem<UIPausePanel>().Show();
        });
    }
}
