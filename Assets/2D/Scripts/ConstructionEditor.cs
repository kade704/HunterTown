using UnityEngine;
using UnityEngine.UI;

public class ConstructionEditor : MonoBehaviour {
    private Construction _selectedConstruction;
    private ConstructionCursor _constructionCursor;
    private Image _blindShade;
    private Button _menuOpenButton;
    private bool _isEditing = false;
    private Construction.Direction _direction;

    public Construction SelectedConstruction {
        get { return _selectedConstruction; }
        set {
            _selectedConstruction = value;
            _constructionCursor.Construction = value;

            if (value) {
                _constructionCursor.SetOutline(false);
            }
        }
    }

    public bool IsEditing => _isEditing;

    private void Awake() {
        _constructionCursor = FindObjectOfType<ConstructionCursor>();

        var menuCanvasGroup = transform.Find("MenuPanel").GetComponent<CanvasGroup>();
        _menuOpenButton = transform.Find("MenuOpenButton").GetComponent<Button>();
        _menuOpenButton.onClick.AddListener(() => {
            UIManager.ShowCanvasGroup(menuCanvasGroup);
            _menuOpenButton.gameObject.SetActive(false);
        });

        var menuCloseButton = transform.Find("MenuPanel/CloseButton").GetComponent<Button>();
        menuCloseButton.onClick.AddListener(() => {
            UIManager.HideCanvasGroup(menuCanvasGroup);
            _menuOpenButton.gameObject.SetActive(true);
        });

        var group = transform.Find("MenuPanel/Group");
        var constructionSlotRef = transform.Find("MenuPanel/ConstructionSlotRef").GetComponent<UIConstructionSlot>();

        var constructions = Resources.LoadAll<Construction>("Constructions");
        foreach (var construction in constructions) {
            var constuctionSlot = Instantiate(constructionSlotRef, group);
            constuctionSlot.Construction = construction;
            constuctionSlot.OnClick.AddListener(() => {
                UIManager.HideCanvasGroup(menuCanvasGroup);
                SelectedConstruction = construction;
                _isEditing = true;

                var color = Color.black; color.a = .5f;
                _blindShade.color = color;
            });
        }

        _blindShade = GameObject.Find("BlindShade").GetComponent<Image>();
    }

    private void Update() {
        var cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var cellPos = ConstructionManager.Instance.WorldToCell(cursor);

        if (_selectedConstruction) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
                SelectedConstruction = null;
                _isEditing = false;

                var color = Color.black; color.a = 0f;
                _blindShade.color = color;

                _menuOpenButton.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                _direction += 1;
                if (_direction >= (Construction.Direction)4) _direction = 0;
                _constructionCursor.Direction = _direction;
            }

            var cursorPos = ConstructionManager.Instance.CellToWorld(cellPos);
            cursorPos.y += .25f;
            _constructionCursor.transform.position = cursorPos;
            _constructionCursor.Error = ConstructionManager.Instance.HasConstruction(cellPos);

            if (Input.GetMouseButtonDown(0) && !UIManager.IsPointerOverUI()) {
                ConstructionManager.Instance.SetConstruction(_selectedConstruction, cellPos, _direction);
                GameManager_.Instance.Money -= _selectedConstruction.Cost;
            }
        }
    }
}
