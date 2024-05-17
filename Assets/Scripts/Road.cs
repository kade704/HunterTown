using System;
using UnityEngine;

[RequireComponent(typeof(Construction))]
public class Road : MonoBehaviour
{
    [Serializable]
    public struct SpriteRule
    {
        [SerializeField] private Sprite _none;
        [SerializeField] private Sprite _swne;
        [SerializeField] private Sprite _swn;
        [SerializeField] private Sprite _swe;
        [SerializeField] private Sprite _sne;
        [SerializeField] private Sprite _wne;
        [SerializeField] private Sprite _sw;
        [SerializeField] private Sprite _sn;
        [SerializeField] private Sprite _se;
        [SerializeField] private Sprite _wn;
        [SerializeField] private Sprite _we;
        [SerializeField] private Sprite _ne;
        [SerializeField] private Sprite _s;
        [SerializeField] private Sprite _w;
        [SerializeField] private Sprite _n;
        [SerializeField] private Sprite _e;

        public Sprite SWNE => _swne;
        public Sprite SWN => _swn;
        public Sprite SWE => _swe;
        public Sprite SNE => _sne;
        public Sprite WNE => _wne;
        public Sprite SW => _sw;
        public Sprite SN => _sn;
        public Sprite SE => _se;
        public Sprite WN => _wn;
        public Sprite WE => _we;
        public Sprite NE => _ne;
        public Sprite S => _s;
        public Sprite W => _w;
        public Sprite N => _n;
        public Sprite E => _e;
        public Sprite None => _none;
    }

    [SerializeField] private float _speed;
    [SerializeField] private SpriteRule _spriteRule;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Construction _construction;
    private Road[] _neighbors = new Road[4];
    public Construction Construction => _construction;
    public SpriteRule SpriteRule_ => _spriteRule;
    public Road[] Neighbors => _neighbors;
    public float Speed => _speed;

    private void Awake()
    {
        _construction = GetComponent<Construction>();
    }

    private void Start()
    {
        UpdateNeighbors();
        UpdateSprite();

        _construction.ConstructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            OnConstructionChanged(construction);
        });

        _construction.ConstructionGridMap.OnConstructionDestroyed.AddListener((construction) =>
        {
            OnConstructionChanged(construction);
        });
    }

    private void OnConstructionChanged(Construction construction)
    {
        var road = construction.GetComponent<Road>();
        if (road != null)
        {
            var xDiff = Mathf.Abs(_construction.CellPos.x - road.Construction.CellPos.x);
            var yDiff = Mathf.Abs(_construction.CellPos.y - road.Construction.CellPos.y);
            if (xDiff <= 1 && yDiff <= 1)
            {
                UpdateNeighbors();
                UpdateSprite();
            }
        }
    }

    private void UpdateSprite()
    {
        var east = _neighbors[1];
        var west = _neighbors[0];
        var north = _neighbors[3];
        var south = _neighbors[2];

        if (south && west && north && east)
        {
            _spriteRenderer.sprite = _spriteRule.SWNE;
        }
        else if (south && west && north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.SWN;
        }
        else if (south && west && !north && east)
        {
            _spriteRenderer.sprite = _spriteRule.SWE;
        }
        else if (south && !west && north && east)
        {
            _spriteRenderer.sprite = _spriteRule.SNE;
        }
        else if (!south && west && north && east)
        {
            _spriteRenderer.sprite = _spriteRule.WNE;
        }
        else if (south && west && !north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.SW;
        }
        else if (south && !west && north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.SN;
        }
        else if (south && !west && !north && east)
        {
            _spriteRenderer.sprite = _spriteRule.SE;
        }
        else if (!south && west && north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.WN;
        }
        else if (!south && west && !north && east)
        {
            _spriteRenderer.sprite = _spriteRule.WE;
        }
        else if (!south && !west && north && east)
        {
            _spriteRenderer.sprite = _spriteRule.NE;
        }
        else if (south && !west && !north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.S;
        }
        else if (!south && west && !north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.W;
        }
        else if (!south && !west && north && !east)
        {
            _spriteRenderer.sprite = _spriteRule.N;
        }
        else if (!south && !west && !north && east)
        {
            _spriteRenderer.sprite = _spriteRule.E;
        }
        else
        {
            _spriteRenderer.sprite = _spriteRule.None;
        }
    }

    public void UpdateNeighbors()
    {
        var x = new[] { -1, 1, 0, 0 };
        var y = new[] { 0, 0, -1, 1 };

        for (var i = 0; i < 4; i++)
        {
            var cellPos = new Vector2Int(_construction.CellPos.x + x[i], _construction.CellPos.y + y[i]);
            _neighbors[i] = _construction.ConstructionGridMap.GetConstructionAt(cellPos)?.GetComponent<Road>();
        }
    }
}
