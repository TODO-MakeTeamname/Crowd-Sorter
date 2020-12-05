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

        JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent) => {
            //Update X position
            translation.Value.x += moveSpeedComponent.moveSpeedX * dt;
            if (translation.Value.x > Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                moveSpeedComponent.moveSpeedX = -Mathf.Abs(moveSpeedComponent.moveSpeedX);
            if (translation.Value.x < -Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                moveSpeedComponent.moveSpeedX = +Mathf.Abs(moveSpeedComponent.moveSpeedX);

            //Update Z position
            translation.Value.z += moveSpeedComponent.moveSpeedZ * dt;
            if (translation.Value.z > Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                moveSpeedComponent.moveSpeedZ = -Mathf.Abs(moveSpeedComponent.moveSpeedZ);
            if (translation.Value.z < -Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                moveSpeedComponent.moveSpeedZ = +Mathf.Abs(moveSpeedComponent.moveSpeedZ);
        }).Schedule(inputDeps);
        return jobHandle;
    }
}


