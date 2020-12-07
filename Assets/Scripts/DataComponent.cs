using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct DataComponent : IComponentData
{
    public float value;
    public bool sorted;
    public bool inPosition;
}