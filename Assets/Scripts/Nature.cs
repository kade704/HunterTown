using UnityEngine;

public class Nature : Construction
{
    [SerializeField] private Sprite[] _randomSprites;


    protected override void Start()
    {
        base.Start();
        var idx = Random.Range(0, _randomSprites.Length - 1);
        _spriteRenderer.sprite = _randomSprites[idx];
    }
}
