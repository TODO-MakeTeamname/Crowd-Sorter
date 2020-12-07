using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

public class SortingSystem : ComponentSystem {
    int index = 0;
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.ForEach((ref DataComponent dataComponent, ref MoveSpeedComponent moveSpeedComponent) =>
        { 
            //check if at correct index and has not already been sorted
            if ((float)index == dataComponent.value && !dataComponent.sorted)
            {
                dataComponent.sorted = true;
                Debug.Log("person at index " + index + " is being sorted");
            }
            //move on when in position
            else if ((float)index == dataComponent.value && dataComponent.inPosition)
            {
                index++;
            }

        });
        

    }
}
