using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnCount;
    private float spawnTime = 0f; // Time interval between spawns
    public GameObject target;

    // spawning box
    public Vector3 spawnAreaMin;         // Minimum point (corner) of the spawn area
    public Vector3 spawnAreaMax;         // Maximum point (corner) of the spawn area

    // Circular spawn area
    private Vector3 spawnCenter;
    public float spawnMinRadius;
    public float spawnMaxRadius;

    private void Start()
    {
        spawnCenter = transform.position;
    }

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            // Create a target entity (this could be done in many different ways, here it's assumed you create a target entity somewhere)
            Entity targetEntity = GetEntity(authoring.target, TransformUsageFlags.Dynamic);

            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add the components related to spawn configuration
            AddComponent(entity, new SpawnEnemyConfig
            {
                enemyPrefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
                spawnCount = authoring.spawnCount,
                spawnTime = authoring.spawnTime,
            }) ;

            // Add the spawn area as a component
            AddComponent(entity, new SpawnArea
            {
                min = authoring.spawnAreaMin,
                max = authoring.spawnAreaMax,

                center = authoring.spawnCenter,
                minRadius = authoring.spawnMinRadius,
                maxRadius = authoring.spawnMaxRadius
            });

            AddComponent(entity, new TargetPosition
            {
                //target = targetEntity
                target = authoring.target.transform.position
            });
        }
    }
}

public struct SpawnEnemyConfig : IComponentData
{
    public Entity enemyPrefab;
    public int spawnCount;
    public float spawnTime;
}

public struct TargetPosition : IComponentData
{
    //public Entity target;
    public float3 target;
}

public struct SpawnArea : IComponentData
{
    public float3 min; // The center point of the spawn area
    public float3 max;  // The radius of the spawn area

    public float3 center; // Center of the circular spawn area
    public float minRadius; // Minimum radius of the circle
    public float maxRadius; // Maximum radius of the circle
}
