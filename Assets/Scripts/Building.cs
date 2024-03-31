using System;
using UnityEngine;

public class Building : Construction
{
    public enum Direction { SOUTH, WEST, NORTH, EAST }

    [Serializable]
    public struct SpriteDirection
    {
        [SerializeField] private Sprite _east;
        [SerializeField] private Sprite _south;
        [SerializeField] private Sprite _west;
        [SerializeField] private Sprite _north;

        public Sprite East => _east;
        public Sprite South => _south;
        public Sprite West => _west;
        public Sprite North => _north;
    }


    public SpriteDirection _spriteDirection;

    protected Direction _direction;

    public Direction Direction_
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
            _spriteRenderer.sprite = GetSpriteFromDirection(value);
        }
    }

    public Sprite GetSpriteFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.EAST:
                return _spriteDirection.East;
            case Direction.WEST:
                return _spriteDirection.West;
            case Direction.SOUTH:
                return _spriteDirection.South;
            case Direction.NORTH:
                return _spriteDirection.North;
            default:
                break;
        }
        return null;
    }
}
