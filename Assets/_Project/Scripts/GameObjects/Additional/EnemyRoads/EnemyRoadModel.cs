using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using SplineContainerData = _Project.Scripts.DTO.SplineContainerData;
using Vector2Scaled = _Project.Scripts.DTO.Vector2Scaled;
using Vector3Scaled = _Project.Scripts.DTO.Vector3Scaled;

namespace _Project.Scripts.GameObjects.Additional.EnemyRoads
{
    [Serializable]
    [MemoryPackable]
    public partial class EnemyRoadModel : ISavableModel
    {
        [MemoryPackInclude][field: SerializeField] public List<EnemyGroup> RoundEnemyList { get; set; } = new();
        [MemoryPackInclude] public SplineContainerData SplineContainerData { get; set; } = new();
        [MemoryPackInclude] public List<Vector3> WorldPositions { get; set; } = new();
        [MemoryPackInclude] public int CurrentIndex { get; set; }
        [MemoryPackInclude] public float ElapsedTime { get; set; }
        [MemoryPackInclude] public Vector3Scaled[] Vertices { get; set; }
        [MemoryPackInclude] public Vector3Scaled[] Normals { get; set; }
        [MemoryPackInclude] public Vector2Scaled[] UVs { get; set; }
        [MemoryPackInclude] public ushort[] Triangles { get; set; }
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        public virtual void LoadData(ISavableModel model)
        {
            if (model is not EnemyRoadModel objectModel) return;
            RoundEnemyList = objectModel.RoundEnemyList;
            SplineContainerData = objectModel.SplineContainerData;
            WorldPositions = objectModel.WorldPositions;
            CurrentIndex = objectModel.CurrentIndex;
            ElapsedTime = objectModel.ElapsedTime;
            Vertices = objectModel.Vertices;
            Normals = objectModel.Normals;
            UVs = objectModel.UVs;
            Triangles = objectModel.Triangles;
            SavePosition = objectModel.SavePosition;
            SaveRotation = objectModel.SaveRotation;
        }
        
        public ISavableModel GetSaveData()
        {
            return new EnemyRoadModel
            {
                RoundEnemyList = RoundEnemyList,
                SplineContainerData = SplineContainerData,
                WorldPositions = WorldPositions,
                CurrentIndex = CurrentIndex,
                ElapsedTime = ElapsedTime,
                Vertices = Vertices,
                Normals = Normals,
                UVs = UVs,
                Triangles = Triangles,
                SavePosition = SavePosition,
                SaveRotation = SaveRotation
            };
        }
    }

    [Serializable]
    [MemoryPackable]
    public partial class EnemyGroup
    {
        [MemoryPackInclude] public List<EnemyWithTime> enemies;
    }

    [Serializable]
    [MemoryPackable]
    public partial class EnemyWithTime
    {
        [MemoryPackInclude][Min(0f)] public float time;
        [MemoryPackInclude] public UnitType enemyType;
    }
}