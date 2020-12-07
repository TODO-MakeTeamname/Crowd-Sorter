using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;


public class SortingSystem : MonoBehaviour {

    public static GameObject[] objectArray;

    protected void Update()
    {
        GameObject temp = objectArray[Testing.index];
        VarsComponent varsComp = temp.GetComponent<VarsComponent>();
        if (!varsComp.Sorted)
        {
            Debug.Log("Person number " + Testing.index + " is being sorted");
            varsComp.Sorted = true;

            varsComp.SortPos = new Vector3((float)(Testing.index % 320) - 160f, 0f, (float)((Testing.index / 320) % 320) - 160f);
        }
        else if (varsComp.InPosition)
        {
            if (Testing.index < Testing.objectArray.Length - 1)
            {
                Testing.index++;
            }
        }
    }
}
