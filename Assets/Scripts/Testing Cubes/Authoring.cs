using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

// Script name must match this
public class Authoring : MonoBehaviour
{
    public float value;


    private class Baker : Baker<Authoring>
    {
        public override void Bake(Authoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new RotateSpeed { value = authoring.value });
        }
    }
}


// Should only hold data and no functions
public struct RotateSpeed : IComponentData
{
    public float value;
}