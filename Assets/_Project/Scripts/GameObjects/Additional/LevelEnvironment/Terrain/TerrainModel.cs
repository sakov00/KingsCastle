using System;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using Vector2Scaled = _Project.Scripts.DTO.Vector2Scaled;
using Vector3Scaled = _Project.Scripts.DTO.Vector3Scaled;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain
{
    [Serializable]
    [MemoryPackable]
    public partial class TerrainModel : ISavableModel
    {
        [MemoryPackInclude] public Vector3Scaled[] Vertices { get; set; }
        [MemoryPackInclude] public Vector3Scaled[] Normals { get; set; }
        [MemoryPackInclude] public Vector2Scaled[] UVs { get; set; }
        [MemoryPackInclude] public ushort[] Triangles { get; set; }
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        public virtual void LoadData(ISavableModel model)
        {
            if (model is not TerrainModel objectModel) return;
            Vertices = objectModel.Vertices;
            Normals = objectModel.Normals;
            UVs = objectModel.UVs;
            Triangles = objectModel.Triangles;
            SavePosition = objectModel.SavePosition;
            SaveRotation = objectModel.SaveRotation;
        }
        
        public ISavableModel GetSaveData()
        {
            return new TerrainModel
            {
                Vertices = Vertices,
                Normals = Normals,
                UVs = UVs,
                Triangles = Triangles,
                SavePosition = SavePosition,
                SaveRotation = SaveRotation,
            };
        }
    }
}