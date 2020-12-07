using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;

public class MovementSystem : MonoBehaviour
{
    public static float moveSpeed = 5f;
    public static GameObject[] objectArray;
    public static bool instantMode = false;
    Vector3 translation;

    private void Update()
    {
        //get object and parameters
        GameObject temp = objectArray[Testing.index];
        VarsComponent varsComp = temp.GetComponent<VarsComponent>();

        //get time
        float dt = Time.deltaTime;
        
        //get transform component
        Transform transform = temp.transform;
        
        if (!instantMode)
        {
            if (varsComp.Sorted && !varsComp.InPosition)
            {
                //create a modifiable translation from transform
                translation = transform.position;

                //reset flag each iteration
                varsComp.InPosition = true;

                //update z value if person needs to be closer to position
                if (translation.z < varsComp.SortPos.z)
                {
                    translation.z += moveSpeed * dt;
                    //if outside, put back in bounds
                    if (translation.z > varsComp.SortPos.z)
                    {
                        translation.z = varsComp.SortPos.z;
                    }
                    else
                    {
                        varsComp.InPosition = false;
                    }
                    
                }

                else if (translation.z > varsComp.SortPos.z)
                {
                    translation.z -= moveSpeed * dt;
                    //if outside, put back in bounds
                    if (translation.z < varsComp.SortPos.z)
                    {
                        translation.z = varsComp.SortPos.z;
                    }
                    else
                    {
                        varsComp.InPosition = false;
                    }
                }

                //update x value if person needs to be closer to position
                if (translation.x < varsComp.SortPos.x)
                {
                    translation.x += moveSpeed * dt;
                    //if outside, put back in bounds
                    if (translation.x > varsComp.SortPos.x)
                    {
                        translation.x = varsComp.SortPos.x;
                    }
                    else
                    {
                        varsComp.InPosition = false;
                    }
                }

                else if (translation.x > varsComp.SortPos.x)
                {
                    translation.x -= moveSpeed * dt;
                    //if outside, put back in bounds
                    if (translation.x < varsComp.SortPos.x)
                    {
                        translation.x = varsComp.SortPos.x;
                    }
                    else
                    {
                        varsComp.InPosition = false;
                    }
                }
                //apply translation
                transform.position = translation;
            }
        }
        else
        {
            if (varsComp.Sorted)
            {
                transform.position = varsComp.SortPos;
                varsComp.InPosition = true;
            }
        }
    }
}

    /*
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        float dt = Time.DeltaTime;
        float cameraSize = Camera.main.orthographicSize;
        float cameraAspect = Camera.main.aspect;

        int populationCount = UniversalData.populationCount;
        float crowdDensity = UniversalData.crowdDensity;
        bool instantMode = UniversalData.instantMode;

        JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref MoveSpeedComponent moveSpeedComponent, ref DataComponent dataComponent)=> {
            //use fluid system if instant mode is not enabled
            if (!instantMode)
            {
                //only move this way if not sorted
                if (!dataComponent.sorted)
                {
                    //Update X position
                    translation.x += moveSpeedComponent.moveSpeedX * dt;
                    if (translation.x > Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                    {
                        //put back in bounds, then reverse speed
                        translation.x = Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                        moveSpeedComponent.moveSpeedX = -Mathf.Abs(moveSpeedComponent.moveSpeedX);
                    }
                    if (translation.x < -Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                    {
                        //put back in bounds, then reverse speed
                        translation.x = -Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                        moveSpeedComponent.moveSpeedX = +Mathf.Abs(moveSpeedComponent.moveSpeedX);
                    }

                    //Update Z position
                    translation.z += moveSpeedComponent.moveSpeedZ * dt;
                    if (translation.z > Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                    {
                        //put back in bounds, then reverse speed
                        translation.z = Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                        moveSpeedComponent.moveSpeedZ = -Mathf.Abs(moveSpeedComponent.moveSpeedZ);
                    }
                    if (translation.z < -Mathf.Sqrt(populationCount) / (2 * crowdDensity))
                    {
                        //put back in bounds, then reverse speed
                        translation.z = -Mathf.Sqrt(populationCount) / (2 * crowdDensity);
                        moveSpeedComponent.moveSpeedZ = +Mathf.Abs(moveSpeedComponent.moveSpeedZ);
                    }
                }
                //makes person head towards sorted position
                else if (!dataComponent.inPosition)
                {
                    //reset flag each iteration
                    dataComponent.inPosition = true;

                    //update z value if person needs to be closer to position
                    if (translation.z < dataComponent.sortPos.z)
                    {
                        translation.z += moveSpeedComponent.moveSpeedZ * dt;
                        //if outside, put back in bounds
                        if (translation.z > dataComponent.sortPos.z)
                        {
                            translation.z = dataComponent.sortPos.z;
                        }
                        moveSpeedComponent.moveSpeedZ = 8;
                        dataComponent.inPosition = false;
                    }

                    else if (translation.z > dataComponent.sortPos.z)
                    {
                        translation.z -= moveSpeedComponent.moveSpeedZ * dt;
                        //if outside, put back in bounds
                        if (translation.z < dataComponent.sortPos.z)
                        {
                            translation.z = dataComponent.sortPos.z;
                        }
                        moveSpeedComponent.moveSpeedZ = 8;
                        dataComponent.inPosition = false;
                    }

                    //update x value if person needs to be closer to position
                    if (translation.x < dataComponent.sortPos.x)
                    {
                        translation.x += moveSpeedComponent.moveSpeedX * dt;
                        //if outside, put back in bounds
                        if (translation.x > dataComponent.sortPos.x)
                        {
                            translation.x = dataComponent.sortPos.x;
                        }
                        moveSpeedComponent.moveSpeedX = 8;
                        dataComponent.inPosition = false;
                    }

                    else if (translation.x > dataComponent.sortPos.x)
                    {
                        translation.x -= moveSpeedComponent.moveSpeedX * dt;
                        //if outside, put back in bounds
                        if (translation.x < dataComponent.sortPos.x)
                        {
                            translation.x = dataComponent.sortPos.x;
                        }
                        moveSpeedComponent.moveSpeedX = 8;
                        dataComponent.inPosition = false;
                    }
                }
            }
        //if instantmode is enabled, don't allow people to move
        else
            {
                if (dataComponent.sorted)
                {
                    translation.z = dataComponent.sortPos.z;
                    translation.x = dataComponent.sortPos.x;
                    dataComponent.inPosition = true;
                }
            }
        }).Schedule(inputDeps);
        return jobHandle;
    }
    
}
    */




