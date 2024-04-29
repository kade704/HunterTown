using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGridMap : MonoBehaviour
{
    public Sprite[] _tiles;
    SpriteRenderer _spriteRenderer;
    Texture2D _texture;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //print(_tiles[0].texture.GetPixel(32, 16));

        _texture = new Texture2D(64 * 16, 32 * 16, UnityEngine.TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point
        };
        for (int y = 0; y < 16; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                SetTile(new Vector2Int(x, y), _tiles[0]);
            }
        }

        var sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), Vector2.one * 0.5f, 64);
        _spriteRenderer.sprite = sprite;
    }

    private void SetTile(Vector2Int position, Sprite tile)
    {
        var u = position.y - position.x;
        var v = position.y + position.x;

        var offX = (64 * 8 - 32) + u * 32;
        var offY = v * 16;

        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                var pixel = tile.texture.GetPixel(0 + x, 80 + y);
                if (pixel.a > 0.1f)
                {
                    _texture.SetPixel(offX + x, offY + y, pixel);
                }
            }
        }

        _texture.Apply();
    }
}
