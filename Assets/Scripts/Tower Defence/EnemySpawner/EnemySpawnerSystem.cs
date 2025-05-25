using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public partial class EnemySpawnerSystem : SystemBase
{
    private float spawnTimer = 0;
    public float pop = 0;
    protected void OnCreate(ref SystemState state)
    {
        RequireForUpdate<SpawnEnemyConfig>();
    }

    protected override void OnUpdate()
    {
        this.Enabled = false;

        var spawnConfig = SystemAPI.GetSingleton<SpawnEnemyConfig>();
        var spawnArea = SystemAPI.GetSingleton<SpawnArea>();
        var targetData = SystemAPI.GetSingleton<TargetPosition>();


        spawnTimer += SystemAPI.Time.DeltaTime;

        // If it's time to spawn an enemy (every 'spawnTime' seconds)
        /*       if (spawnTimer >= spawnConfig.spawnTime)
               {

                   pop++;
                   spawnTimer = 0f; // Reset the timer

                   //Debug.Log("Population: " + pop);

                   // Generate a random position within the spawn area
                   float3 spawnPosition = new float3(
                       UnityEngine.Random.Range(spawnArea.min.x, spawnArea.max.x),
                       UnityEngine.Random.Range(spawnArea.min.y, spawnArea.max.y),
                       UnityEngine.Random.Range(spawnArea.min.z, spawnArea.max.z)
                   );

                   Entity enemy = EntityManager.Instantiate(spawnConfig.enemyPrefab);

                   SystemAPI.SetComponent(enemy, new LocalTransform
                   {
                       Position = spawnPosition,
                       Rotation = quaternion.identity,
                       Scale = 1f
                   });

                   // Set the enemy's position
                   SystemAPI.SetComponent(enemy, new EnemyPosition { translation = spawnPosition });

                   // Assign the target (e.g., the tower)
                   //SystemAPI.SetComponent(enemy, new TargetPosition { targetPos = spawnConfig.target });
               }*/

        // MASS SPAWNING
        Unity.Collections.NativeArray<Entity> OutputEntity = new Unity.Collections.NativeArray<Entity>(spawnConfig.spawnCount, Allocator.Persistent);
        EntityManager.Instantiate(spawnConfig.enemyPrefab, OutputEntity);

        foreach (Entity entity in OutputEntity)
        {
            float radius = UnityEngine.Random.Range(spawnArea.minRadius, spawnArea.maxRadius);
            float angle = UnityEngine.Random.Range(0f, 360f);

            float3 spawnPosition = spawnArea.center + new float3(
                radius * Mathf.Cos(angle),
                UnityEngine.Random.Range(0, 100f), // Adjust this if you want vertical variation
                radius * Mathf.Sin(angle)
            );

            SystemAPI.SetComponent(entity, new LocalTransform
            {
                Position = spawnPosition,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            // Set the enemy's position
            SystemAPI.SetComponent(entity, new EnemyPosition { translation = spawnPosition });


        }

        /*foreach (Entity entity in OutputEntity)
        {
            float3 spawnPosition = new float3(
                UnityEngine.Random.Range(spawnArea.min.x, spawnArea.max.x),
                UnityEngine.Random.Range(spawnArea.min.y, spawnArea.max.y),
                UnityEngine.Random.Range(spawnArea.min.z, spawnArea.max.z)
            );

            SystemAPI.SetComponent(entity, new LocalTransform
            {
                Position = spawnPosition,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            // Set the enemy's position
            SystemAPI.SetComponent(entity, new EnemyPosition { translation = spawnPosition });
        }*/
    }
}

