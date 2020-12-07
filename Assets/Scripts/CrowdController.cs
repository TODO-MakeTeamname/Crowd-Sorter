using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

public class CrowdController : MonoBehaviour {
    public int population = 1000;
    public float crowdDensity = 1f;

    // i and j for manual bubble sort for loops
    private int i = 0;
    private int j = 0;

    public List<PersonController> people;

    public GameObject personPrefab;

    public void Start() {
        SpawnPeople(population);
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
        Debug.Log("");
    }

    public void FixedUpdate() {
        bubbleSort();
    }

    private void bubbleSort() {
        if (i < people.Count) {
            if (j < people.Count - 1) {
                if (people[j].value > people[j + 1].value) {
                    // swap in list
                    var personTemp = people[j];
                    people[j] = people[j + 1];
                    people[j + 1] = personTemp;
                    // swap in game
                    var positionTemp = people[j].transform.position;
                    people[j].transform.position = people[j + 1].transform.position;
                    people[j + 1].transform.position = positionTemp;
                }
                j++;
            } else {
                i++;
                j = 0;
            }
        }
    }
}
