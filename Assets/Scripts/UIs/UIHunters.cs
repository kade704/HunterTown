using UnityEngine;

public class UIHunters : MonoBehaviour
{
    [SerializeField] private UIHunterSlot _hunterSlotPrefab;
    [SerializeField] private Transform _hunterSlotContainer;

    private void Awake()
    {
        var hunterSpawner = FindObjectOfType<HunterSpawner>();
        hunterSpawner.OnHuntersChanged.AddListener(() =>
        {
            foreach (Transform child in _hunterSlotContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var hunter in hunterSpawner.Hunters)
            {
                var slot = Instantiate(_hunterSlotPrefab, _hunterSlotContainer);
                slot.Hunter = hunter;
            }
        });
    }
}
