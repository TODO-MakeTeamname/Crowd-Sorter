using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        float dt = Time.DeltaTime;
        float cameraSize = Camera.main.orthographicSize;
        float cameraAspect = Camera.main.aspect;

        int populationCount = UniversalData.populationCount;
        float crowdDensity = UniversalData.crowdDensity;

        JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent, ref SortableComponent sortable)=> {
            //only move this way if not sorted
            if (!sortable.sorted)
            {
                //Update X position
                translation.Value.x += moveSpeedComponent.moveSpeedX * dt;
                if (translation.Value.x > Mathf.Sqrt(populationCount) / (2 * crowdDensity)) {
                    //put back in bounds, then reverse speed
                    translation.Value.x = Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                    moveSpeedComponent.moveSpeedX = -Mathf.Abs(moveSpeedComponent.moveSpeedX);
                }
                if (translation.Value.x < -Mathf.Sqrt(populationCount) / (2 * crowdDensity)) {
                    //put back in bounds, then reverse speed
                    translation.Value.x = -Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                    moveSpeedComponent.moveSpeedX = +Mathf.Abs(moveSpeedComponent.moveSpeedX);
                }

                //Update Z position
                translation.Value.z += moveSpeedComponent.moveSpeedZ * dt;
                if (translation.Value.z > Mathf.Sqrt(populationCount) / (2 * crowdDensity)) {
                    //put back in bounds, then reverse speed
                    translation.Value.z = Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                    moveSpeedComponent.moveSpeedZ = -Mathf.Abs(moveSpeedComponent.moveSpeedZ);
                }
                if (translation.Value.z < -Mathf.Sqrt(populationCount) / (2 * crowdDensity)) {
                    //put back in bounds, then reverse speed
                    translation.Value.z = -Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                    moveSpeedComponent.moveSpeedZ = +Mathf.Abs(moveSpeedComponent.moveSpeedZ);
                }
            }
            else if (!sortable.inPosition)
            {
                //reset flag each iteration
                sortable.inPosition = true;

                //update z value if person needs to be closer to position
                if (translation.Value.z < Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2)
                {
                    translation.Value.z += moveSpeedComponent.moveSpeedZ * dt;
                    //if outside, put back in bounds
                    if (translation.Value.z > Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2)
                    {
                        translation.Value.z = Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2;
                    }
                    moveSpeedComponent.moveSpeedZ = 5;
                    sortable.inPosition = false;
                }

                //update x value if person needs to be closer to position
                if (translation.Value.x < Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2)
                {
                    translation.Value.x += moveSpeedComponent.moveSpeedX * dt;
                    //if outside, put back in bounds
                    if (translation.Value.x > Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2)
                    {
                        translation.Value.x = Mathf.Sqrt(populationCount) / (2 * crowdDensity) + 2;
                    }
                    moveSpeedComponent.moveSpeedX = 5;
                    sortable.inPosition = false;
                }
            }
        }).Schedule(inputDeps);
        return jobHandle;
    }
}


