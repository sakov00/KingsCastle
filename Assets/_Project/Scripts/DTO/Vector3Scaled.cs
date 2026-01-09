using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.DTO
{
    [MemoryPackable]
    public partial struct Vector3Scaled
    {
        public short x;
        public short y;
        public short z;
        private const float Scale = 100f;

        public Vector3Scaled(Vector3 v)
        {
            x = (short)(v.x * Scale);
            y = (short)(v.y * Scale);
            z = (short)(v.z * Scale);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(
                x / Scale,
                y / Scale,
                z / Scale
            );
        }
    }
}