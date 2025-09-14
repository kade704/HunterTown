using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHunters : MonoBehaviour
{
    [SerializeField] private UIHunterButton _hunterButtonPrefab;

    private Dictionary<Hunter, UIHunterButton> _hunterButtonMap = new();

    private void Start()
    {
        StartCoroutine(OnHunterChanged());

        GameManager.Instance.GetSystem<HunterSpawner>().OnHuntersChanged.AddListener(() => StartCoroutine(OnHunterChanged()));
    }

    private IEnumerator OnHunterChanged()
    {
        yield return null;
        yield return null;

        var hunterButtons = _hunterButtonMap.Values.ToList();

        foreach (var hunter in GameManager.Instance.GetSystem<HunterSpawner>().Hunters)
        {
            if (!_hunterButtonMap.ContainsKey(hunter))
            {
                var button = Instantiate(_hunterButtonPrefab, transform);
                button.Hunter = hunter;
                button.OnClick.AddListener(() =>
                {
                    GameManager.Instance.GetSystem<InteractableSelector>().SelectInteractable(hunter.GetComponent<Interactable>());
                    GameManager.Instance.GetSystem<CameraMovement>().MovePosition(hunter.transform.position);
                });
                _hunterButtonMap.Add(hunter, button);
            }
            else
            {
                hunterButtons.Remove(_hunterButtonMap[hunter]);
            }
        }

        foreach (var hunterButton in hunterButtons)
        {
            _hunterButtonMap.Remove(_hunterButtonMap.First(x => x.Value == hunterButton).Key);
            Destroy(hunterButton.gameObject);
        }
    }
}
