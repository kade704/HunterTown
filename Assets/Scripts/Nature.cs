using UnityEngine;

public class Nature : MonoBehaviour
{
    [SerializeField] private Sprite[] _randomSprites;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var idx = Random.Range(0, _randomSprites.Length - 1);
        _spriteRenderer.sprite = _randomSprites[idx];
    }
}
