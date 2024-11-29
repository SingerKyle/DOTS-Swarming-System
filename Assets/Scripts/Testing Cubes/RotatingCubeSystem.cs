using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// non entity code = IJobFor
/// 
/// ref keyword = read write
///           in = read only
/// </summary>
[BurstCompile]
[WithAll(typeof(RotatingCube))]
public partial struct RotateCubeJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
    {
        localTransform = localTransform.RotateY(rotateSpeed.value * deltaTime);
    }
}

/// <summary>
/// partial Class SystemBase Extension = used for managed data
/// partial Struct ISystem = unmanaged types (bool, struct, int)
/// </summary>
public partial struct RotatingCubeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    public void OnDestroy(ref SystemState state)
    {

    }

    /// <summary>
    ///  RefRW = Read Write
    ///  RefRO = Read Only
    /// </summary>
    /// <param name="state"></param>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        return;
        //                                                                                                                                             Or WithAll<RotatingCube>
        //foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>().WithNone<Player>())
        //{
        //    localTransform.ValueRW = localTransform.ValueRW.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
        //}

        RotateCubeJob rotatingCubeJob = new RotateCubeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        //rotatingCubeJob.Run() will run on main thread
        //rotatingCubeJob.Schedule() sets it to be run on one thread
        //rotatingCubeJob.ScheduleParallel() splits into chunks to run on many threads
        // TO FORCE A JOB add dependency:
        //  rotatingCubeJob.Schedule(state.Dependency).Complete();
        rotatingCubeJob.ScheduleParallel();
    }

}
