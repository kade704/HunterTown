using UnityEngine;
using UnityEngine.UI;

public class UIWaveWarning : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        var monsterSpawner = FindObjectOfType<MonsterSpawner>();
        _image.enabled = monsterSpawner.Monsters.Length > 0;
    }
}
