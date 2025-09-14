using UnityEngine;
using UnityEngine.Events;

public class GameOverSystem : MonoBehaviour
{
    private UnityEvent<string> _onGameOver = new();

    public UnityEvent<string> OnGameOver => _onGameOver;

    private void Start()
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();

        var constructionGridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();
        constructionGridmap.OnConstructionDestroyed.AddListener((construction) =>
        {
            if (construction.GetComponent<Company>() != null)
            {
                GameOver("본부 건물이 파괴되었습니다");
            }
        });


        timeSystem.Month.OnChanged.AddListener(() =>
        {
            var moneySystem = GameManager.Instance.GetSystem<MoneySystem>();
            if (moneySystem.Money < 0)
            {
                GameOver("파산했습니다");
            }
        });
    }

    public void GameOver(string message)
    {
        var timeSystem = GameManager.Instance.GetSystem<TimeSystem>();

        timeSystem.Pause();
        _onGameOver.Invoke(message);
    }
}
