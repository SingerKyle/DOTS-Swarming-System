using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnCubesConfigAuthoring : MonoBehaviour
{
    public GameObject cubePrefab;

    public int spawnCount;


    public class Baker : Baker<SpawnCubesConfigAuthoring>
    {
        public override void Bake(SpawnCubesConfigAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnCubesConfig { cubePrefabEntity = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic), spawnCount = authoring.spawnCount });
                {
                
            }
        }
    }
}

public struct SpawnCubesConfig : IComponentData
{
    public Entity cubePrefabEntity;
    public int spawnCount;
}