using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotateCubeAuthoring : MonoBehaviour
{
    public class Baker : Baker<RotateCubeAuthoring>
    {
        public override void Bake(RotateCubeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new RotatingCube());
        }
    }
}

public struct RotatingCube : IComponentData
{

}