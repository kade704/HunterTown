using UnityEngine;

public class ConstructionCursor : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private Construction _construction;

    public Construction Construction {
        set {
            if (value) {
                _spriteRenderer.sprite = value.Icon;
            } else {
                _spriteRenderer.sprite = null;
            }
            _construction = value;
        }
    }

    public Building.Direction Direction {
        set {
            if (_construction && _construction is Building) {
                _spriteRenderer.sprite = ((Building)_construction).GetSpriteFromDirection(value);
            }
        }
    }

    public bool Error {
        set {
            _spriteRenderer.color = value ? Color.red : Color.white;
        }
    }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
