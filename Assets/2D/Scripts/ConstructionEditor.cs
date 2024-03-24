using UnityEngine;
using UnityEngine.UI;

public class ConstructionEditor : MonoBehaviour {
    private Construction _constructionToBuild;
    private ConstructionCursor _constructionCursor;
    private Image _blindShade;
    private Button _menuOpenButton;
    private bool _isEditing = false;
    private Building.Direction _direction;
    private Vector2Int _cellPos;

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
                _constructionToBuild = construction;
                _isEditing = true;

                var color = Color.black; color.a = .5f;
                _blindShade.color = color;
            });
        }

        _blindShade = GameObject.Find("BlindShade").GetComponent<Image>();
    }

    private void Update() {
        var cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _cellPos = ConstructionManager.Instance.WorldToCell(cursor);

        if (_constructionToBuild) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
                _constructionToBuild = null;
                _isEditing = false;

                var color = Color.black; color.a = 0f;
                _blindShade.color = color;

                _menuOpenButton.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                _direction += 1;
                if (_direction >= (Building.Direction)4) _direction = 0;
                _constructionCursor.Direction = _direction;
            }

            var buildable = CheckBuildable();

            var cursorPos = ConstructionManager.Instance.CellToWorld(_cellPos);
            cursorPos.y += .25f;
            _constructionCursor.transform.position = cursorPos;
            _constructionCursor.Construction = _constructionToBuild;
            _constructionCursor.Direction = _direction;
            _constructionCursor.Error = !buildable;

            if (Input.GetMouseButtonDown(0) && !UIManager.IsPointerOverUI() && buildable) {
                if (_constructionToBuild is Building) {
                    var newBuilding = ConstructionManager.Instance.BuildingMap.SetConstruction(_constructionToBuild, _cellPos) as Building;
                    newBuilding.Direction_ = _direction;
                } else if (_constructionToBuild is Road) {
                    ConstructionManager.Instance.RoadMap.SetConstruction(_constructionToBuild, _cellPos);
                }

                GameManager_.Instance.Money -= _constructionToBuild.Cost;
            }
        }
    }

    private bool CheckBuildable() {
        var result = true;
        if (ConstructionManager.Instance.HasConstruction(_cellPos)) {
            result = false;
        }
        if (_constructionToBuild is Building) {
            if (_direction == Building.Direction.SOUTH) {
                if (!ConstructionManager.Instance.RoadMap.HasConstruction(new Vector2Int(_cellPos.x - 1, _cellPos.y))) {
                    result = false;
                }
            } else if (_direction == Building.Direction.NORTH) {
                if (!ConstructionManager.Instance.RoadMap.HasConstruction(new Vector2Int(_cellPos.x + 1, _cellPos.y))) {
                    result = false;
                }
            } else if (_direction == Building.Direction.EAST) {
                if (!ConstructionManager.Instance.RoadMap.HasConstruction(new Vector2Int(_cellPos.x, _cellPos.y - 1))) {
                    result = false;
                }
            } else if (_direction == Building.Direction.WEST) {
                if (!ConstructionManager.Instance.RoadMap.HasConstruction(new Vector2Int(_cellPos.x, _cellPos.y + 1))) {
                    result = false;
                }
            }
        }
        return result;
    }
}
