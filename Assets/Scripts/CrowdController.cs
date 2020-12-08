using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

public class CrowdController : MonoBehaviour {
    public int population = 1000;
    public float crowdDensity = 1f;

    public bool useBubbleSort = true;

    // i and j for manual bubble sort/insertion sort for loops
    private int i;
    private int j;

    public List<PersonController> people;

    public GameObject personPrefab;

    public void Start() {
        SpawnPeople(population);
        if (useBubbleSort) {
            i = 0;
            j = 0;
        } else {
            i = 0;
            j = i + 1;
        }
    }

    public void SpawnPeople(int population) {
        for (int i = 0; i < population; i++) {
            //Initialize Positions
            float rowOffset = (Mathf.Sqrt(population) / (2 * crowdDensity));
            float x = ((i % Mathf.Sqrt(population)) / crowdDensity) - rowOffset;
            float z = (i / (Mathf.Sqrt(population) * crowdDensity)) - rowOffset;
            var position = new Vector3(x, 0, z);

            var person = Instantiate(personPrefab, position, Quaternion.identity) as GameObject;
            var controller = person.GetComponent<PersonController>();

            //Set scale/value
            var scale = UnityEngine.Random.Range(1f, 2f);
            person.transform.localScale = new Vector3(
                    scale,
                    scale,
                    scale
            );
            controller.value = scale;

            people.Add(controller);
        }
    }

    public void FixedUpdate() {
        if (useBubbleSort) {
            BubbleSort();
        } else {
            SelectionSort();
        }

    }

    private void BubbleSort() {
        if (i < people.Count) {
            if (j < people.Count - 1) {
                if (people[j].value > people[j + 1].value) {
                    swap(j,j+1);
                }
                j++;
            } else {
                i++;
                j = 0;
            }
        }
    }


    private int minIndex = 0; // min index for selection sort

    private void SelectionSort() {
        Debug.Log($"i: {i} j: {j}");
        if (i < people.Count - 1) {
            if (j < people.Count) {
                if (people[j].value < people[minIndex].value) {
                    minIndex = j;
                }
                j++;
            } else {
                swap(i, minIndex);
                i++;
                minIndex = i;
                j = i + 1;
            }
        } 
    }

    private void swap(int i, int j) {
        // swap in list
        var personTemp = people[j];
        people[j] = people[i];
        people[i] = personTemp;
        // swap in game
        var positionTemp = people[j].transform.position;
        people[j].transform.position = people[i].transform.position;
        people[i].transform.position = positionTemp;
    }

}
