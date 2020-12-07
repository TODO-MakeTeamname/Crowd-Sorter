using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SortableComponent : IComponentData {
    public float value;
    public bool sorted;
    public bool inPosition;
}