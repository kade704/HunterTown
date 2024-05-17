using System.Collections.Generic;
using UnityEngine;

public class UIHunters : MonoBehaviour
{
    [SerializeField] private UIHunterButton _hunterButtonPrefab;

    private List<UIHunterButton> _hunterButtons = new();

    private void Awake()
    {
        var hunterSpawner = FindObjectOfType<HunterSpawner>();
        hunterSpawner.OnHuntersChanged.AddListener(() =>
        {
            _hunterButtons.Clear();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var hunter in hunterSpawner.Hunters)
            {
                var button = Instantiate(_hunterButtonPrefab, transform);
                button.Hunter = hunter;
                button.OnClick.AddListener(() =>
                {
                    GameManager.Instance.GetSystem<InteractableSelector>().SelectInteractable(hunter.GetComponent<Interactable>());
                    GameManager.Instance.GetSystem<CameraMovement>().MovePosition(hunter.transform.position);
                });
                _hunterButtons.Add(button);
            }
        });
    }
}
