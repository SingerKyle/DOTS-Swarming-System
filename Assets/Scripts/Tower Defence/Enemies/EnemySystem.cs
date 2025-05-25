using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using System;


[BurstCompile]
[WithAll(typeof(Enemy))]
public partial struct MoveEnemyJob : IJobEntity
{
    public float deltaTime;
    public float3 targetPosition;

    public void Execute(ref LocalTransform localTransform, in EnemySpeed speed)
    {
        float3 direction = math.normalize(targetPosition - localTransform.Position);
        localTransform.Position += direction * speed.speed * deltaTime;
    }
}

[BurstCompile]
[WithAll(typeof(Enemy))]
public partial struct CheckAndDestroyEnemyJob : IJobEntity
{
    public float3 targetPosition;
    public EntityCommandBuffer ecb;

    public void Execute(ref LocalTransform localTransform, Entity entity)
    {
        if (math.distance(targetPosition, localTransform.Position) <= 10f)
        {
            // No need for entityInQueryIndex in DOTS 1.0+ jobs.
            ecb.DestroyEntity(entity);
        }
    }
}

[BurstCompile]
//[WithAll(typeof(Enemy))]
public partial class EnemySystem : SystemBase
{
    private EntityCommandBufferSystem _entityCommandBufferSystem;

    protected override void OnCreate()
    {
        RequireForUpdate<EnemyPosition>();

        _entityCommandBufferSystem = World.GetOrCreateSystemManaged<EntityCommandBufferSystem>();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Enabled = true;

        //                                                                                                                                          Or WithAll<RotatingCube>
        //foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithNone<Player>())
        //{
        //    localTransform.ValueRW = localTransform.ValueRW.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
        //}

        // Fetch the target position from the singleton
        float3 targetPosition = SystemAPI.GetSingleton<TargetPosition>().target;

        //float3 newTargetPosition = new float3(targetPosition.x, targetPosition.y, targetPosition.z + UnityEngine.Random.Range( - 500f, + 500f));

        MoveEnemyJob moveEnemyJob = new MoveEnemyJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            targetPosition = targetPosition
        };

        //rotatingCubeJob.Run() will run on main thread
        //rotatingCubeJob.Schedule() sets it to be run on one thread
        //rotatingCubeJob.ScheduleParallel() splits into chunks to run on many threads
        // TO FORCE A JOB add dependency:
        moveEnemyJob.Schedule(Dependency).Complete();
        //moveEnemyJob.ScheduleParallel();

        // Create the EntityCommandBuffer (ECB) with parallel write capability
        EntityCommandBuffer ecb = _entityCommandBufferSystem.CreateCommandBuffer();

        CheckAndDestroyEnemyJob checkAndDestroyEnemyJob = new CheckAndDestroyEnemyJob
        {
            targetPosition = targetPosition,
            ecb = ecb
        };

        checkAndDestroyEnemyJob.Schedule();

        _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
