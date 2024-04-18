using System;
using UnityEngine;

public class Road : Construction
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

    [SerializeField] private SpriteRule _spriteRule;

    protected override void Start()
    {
        UpdateSprite();

        _constructionGridMap.OnConstructionBuilded.AddListener((construction) =>
        {
            if (construction is Road)
            {
                UpdateSprite();
            }
        });
    }

    public void UpdateSprite()
    {
        var east = _constructionGridMap.IsRoadExistAt(new Vector2Int(_cellPos.x + 1, _cellPos.y));
        var west = _constructionGridMap.IsRoadExistAt(new Vector2Int(_cellPos.x - 1, _cellPos.y));
        var north = _constructionGridMap.IsRoadExistAt(new Vector2Int(_cellPos.x, _cellPos.y + 1));
        var south = _constructionGridMap.IsRoadExistAt(new Vector2Int(_cellPos.x, _cellPos.y - 1));

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
}
