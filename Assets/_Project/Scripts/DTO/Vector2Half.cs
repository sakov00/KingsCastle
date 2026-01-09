using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.DTO
{
    [MemoryPackable]
    public partial struct Vector2Half
    {
        public ushort x;
        public ushort y;
        
        public Vector2Half(Vector2 v)
        {
            x = Mathf.FloatToHalf(v.x);
            y = Mathf.FloatToHalf(v.y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(
                Mathf.HalfToFloat(x),
                Mathf.HalfToFloat(y)
            );
        }
    }
}