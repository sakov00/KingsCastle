using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.DTO
{
    [MemoryPackable]
    public partial struct Vector2Scaled
    {
        public short x;
        public short y;
        private const float Scale = 100f;
        
        public Vector2Scaled(Vector2 v)
        {
            x = (short)(v.x * Scale);
            y = (short)(v.y * Scale);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(
                x / Scale,
                y / Scale
            );
        }
    }
}