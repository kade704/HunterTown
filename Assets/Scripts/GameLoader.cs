using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.GetSystem<SaveFileSystem>().HasSaveFile())
        {
            GameManager.Instance.GetSystem<SaveFileSystem>().LoadGame();
        }
        else
        {
            SetupNewGame();
        }
    }

    private void SetupNewGame()
    {
        var player = GameManager.Instance.GetSystem<MoneySystem>();
        player.Money = 10000;

        var constructionGridMap = GameManager.Instance.GetSystem<ConstructionGridmap>();
        var company = GameManager.Instance.GetSystem<ConstructionDatabase>().GetConstructionPrefab("#company");
        company.GetComponent<Company>().RemainEmployeeCount = 4;
        constructionGridMap.BuildConstruction(company, new Vector2Int(16, 16));

        GameManager.Instance.GetSystem<UITutorialPanel>().Show();
    }
}
