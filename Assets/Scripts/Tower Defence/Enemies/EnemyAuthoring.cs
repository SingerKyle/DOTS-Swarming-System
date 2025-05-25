using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class EnemyAuthoring : MonoBehaviour
{
    public float speed;

    public float health = 10;


    private class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            // Create a target entity (this could be done in many different ways, here it's assumed you create a target entity somewhere)
            //Entity targetEntity = Entity.Null;

            //targetEntity = GetEntity(authoring.target, TransformUsageFlags.Dynamic);

            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Enemy());
            AddComponent(entity, new EnemySpeed { speed = authoring.speed});
            AddComponent(entity, new EnemyPosition { translation = authoring.transform.position });
            AddComponent(entity, new EnemyHealth { health = authoring.health });
        }
    }
}

// used to give enemy tag
public struct Enemy : IComponentData
{
    
}

public struct EnemySpeed : IComponentData
{
    public float speed;
}

public struct EnemyPosition : IComponentData
{
    public float3 translation;
}

public struct EnemyHealth : IComponentData
{
    public float health;
}

