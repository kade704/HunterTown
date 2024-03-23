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

    public Construction.Direction Direction {
        set {
            _spriteRenderer.sprite = _construction.GetSpriteFromDirection(value);
        }
    }

    public bool Error {
        set {
            _spriteRenderer.color = value ? Color.red : Color.white;
        }
    }

    public void SetOutline(bool outline) {
        _spriteRenderer.material.SetFloat("_Outline", outline ? 1 : 0);
    }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
