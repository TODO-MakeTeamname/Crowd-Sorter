using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

public class Testing : MonoBehaviour
{
    //adjustable params
    public int populationCount = 1000;
    public GameObject selector;
    public bool instantMode = false;

    //params used by other components
    public static GameObject[] objectArray;
    public static int index = 0;

    private void Start() {
        float border = Mathf.Sqrt(populationCount) / 2;
        objectArray = new GameObject[populationCount];

        for (int i = 0; i < populationCount; i++)
        {
            GameObject temp = Instantiate(selector, new Vector3(UnityEngine.Random.Range(-border, border), 0f, UnityEngine.Random.Range(-border, border)), Quaternion.identity) as GameObject;
            temp.GetComponent<VarsComponent>().InPosition = false;
            temp.GetComponent<VarsComponent>().Sorted = false;
            
            //set value
            float red = UnityEngine.Random.Range(0f, 1f);
            float green = UnityEngine.Random.Range(0f, 1f);
            float blue = UnityEngine.Random.Range(0f, 1f);

            temp.GetComponent<VarsComponent>().Value = (red * 100f) + (green * 10f) + blue;

            //set color to value
            Renderer rend = temp.GetComponent<Renderer>();
            rend.material.SetColor("_Color", new Color(red, green, blue, 1));
            objectArray[i] = temp;
        }

        //copy array references
        SortingSystem.objectArray = objectArray;
        MovementSystem.objectArray = objectArray;
    }

    private void Update()
    {
        if (instantMode && !MovementSystem.instantMode)
        {
            MovementSystem.instantMode = true;
        }

        else if (!instantMode && MovementSystem.instantMode)
        {
            MovementSystem.instantMode = false;
        }
    }
}
