using UnityEngine;
using UnityEngine.Events;

public class GameClearSystem : MonoBehaviour
{
    private UnityEvent _onGameClear = new UnityEvent();

    public UnityEvent OnGameClear => _onGameClear;

    private void Start()
    {
        var populationSystem = GameManager.Instance.GetSystem<PopulationSystem>();
        populationSystem.OnPopulationChanged.AddListener((population) =>
        {
            if (population >= 400)
            {
                GameManager.Instance.GetSystem<TimeSystem>().Pause();
                _onGameClear.Invoke();
            }
        });
    }
}
