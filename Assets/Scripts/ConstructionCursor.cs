using UnityEngine;

public class ConstructionCursor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Construction _targetConstruction;

    public Construction TargetConstruction
    {
        set
        {
            if (value is Road)
            {
                _spriteRenderer.sprite = value.Icon;
            }
            else
            {
                _spriteRenderer.sprite = null;
            }
            _targetConstruction = value;
        }
    }

    public Building.Direction Direction
    {
        set
        {
            if (_targetConstruction && _targetConstruction is Building)
            {
                _spriteRenderer.sprite = ((Building)_targetConstruction).GetSpriteFromDirection(value);
            }
        }
    }


    public bool Error
    {
        set
        {
            _spriteRenderer.color = value ? Color.red : Color.white;
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
