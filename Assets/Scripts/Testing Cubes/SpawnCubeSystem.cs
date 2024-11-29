using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class NewBehaviourScript : SystemBase
{
    protected void OnCreate(ref SystemState state)
    {
        RequireForUpdate<SpawnCubesConfig>();
    }

    protected override void OnUpdate()
    {
        this.Enabled = false;

        SpawnCubesConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();

        // Iterate and instatiate
        /*for (int i = 0; i < spawnCubesConfig.spawnCount; i++) 
        {
            Entity spawnedEntity = EntityManager.Instantiate(spawnCubesConfig.cubePrefabEntity);

            EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new Unity.Mathematics.float3(UnityEngine.Random.Range(-30f, +20f), UnityEngine.Random.Range(-50f, +50f), UnityEngine.Random.Range(-20f, +20f)),
                Rotation = quaternion.identity,
                Scale = 1f
            });
            // or:
            // LocalTransform.FromPosition(new float3(UnityEngine.Random.Range(-10f, +5f), 0, UnityEngine.Random.Range(-4f, +7f)));

        }*/
        // for more optimal performance instantiate all at once then iterate through to set values
        Unity.Collections.NativeArray<Entity> OutputEntity = new Unity.Collections.NativeArray<Entity>(spawnCubesConfig.spawnCount, Allocator.Persistent);
        EntityManager.Instantiate(spawnCubesConfig.cubePrefabEntity, OutputEntity);

        foreach (Entity entity in OutputEntity)
        {
            SystemAPI.SetComponent(entity, new LocalTransform
            {
                Position = new Unity.Mathematics.float3(UnityEngine.Random.Range(-30f, +20f), UnityEngine.Random.Range(-50f, +50f), UnityEngine.Random.Range(-20f, +20f)),
                Rotation = quaternion.identity,
                Scale = 1f
            });

            //EntityManager.AddComponent();
        }

    }
}
