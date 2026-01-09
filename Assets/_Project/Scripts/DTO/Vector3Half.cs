using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.DTO
{
    [MemoryPackable]
    public partial struct Vector3Half
    {
        public ushort x;
        public ushort y;
        public ushort z;

        public Vector3Half(Vector3 v)
        {
            x = Mathf.FloatToHalf(v.x);
            y = Mathf.FloatToHalf(v.y);
            z = Mathf.FloatToHalf(v.z);
        }

        public UnityEngine.Vector3 ToVector3()
        {
            return new Vector3(
                Mathf.HalfToFloat(x),
                Mathf.HalfToFloat(y),
                Mathf.HalfToFloat(z)
            );
        }
    }
}