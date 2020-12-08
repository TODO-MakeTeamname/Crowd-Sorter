using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.UI;

public class CrowdController : MonoBehaviour {
    public int population = 1000;
    public float crowdDensity = 1f;

    private bool useBubbleSort = true;

    // i and j for manual bubble sort/insertion sort for loops
    private int i;
    private int j;

    public List<PersonController> people;
    public GameObject personPrefab;

    private bool bSimulating = false;
    private bool bSorted = false;

    //UI
    public Slider populationSlider;
    public Text populationText;
    public Button startButton;
    public Button stopButton;
    public Button bubbleButton;
    public Button selectionButton;
    public Text runTimeText;

    //Performance
    private float runTime = 0f;


    private void Start() {
        SetSortType(true);
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
        if (bSimulating) {
            if (useBubbleSort) {
                BubbleSort();
            } else {
                SelectionSort();
            }
        }
    }

    private void Update() {
        //If simulating and not sorted
        if (bSimulating && !bSorted) {
            runTime += Time.deltaTime;
            runTimeText.text = runTime.ToString();
        }
    }

    //Bubble Sort
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
        else {
            bSorted = true;
        }
    }


    //Selection Sort
    private int minIndex = 0; // min index for selection sort
    private void SelectionSort() {
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
        else {
            bSorted = true;
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


    //UI
    public void StartSimulation() {
        population = Mathf.RoundToInt(populationSlider.value);
        SpawnPeople(population);

        if (useBubbleSort) {
            i = 0;
            j = 0;
        }
        else {
            i = 0;
            j = i + 1;
        }

        startButton.interactable = false;
        stopButton.interactable = true;
        populationSlider.interactable = false;

        bSimulating = true;
        bSorted = false;
        runTime = 0;
    }
    public void EndSimulation() {
        //Delete gameobjects
        foreach(PersonController person in people)
            GameObject.Destroy(person.gameObject);
        //Clear container
        people.Clear();

        startButton.interactable = true;
        stopButton.interactable = false;
        populationSlider.interactable = true;

        bSimulating = false;
        bSorted = false;
    }

    public void SetSortType(bool bUseBubble) {
        if (bSimulating)
            return;

        if (bUseBubble) {
            useBubbleSort = true;
            bubbleButton.GetComponent<Image>().color = Color.cyan;
            selectionButton.GetComponent<Image>().color = Color.white;
        }
        else {
            useBubbleSort = false;
            selectionButton.GetComponent<Image>().color = Color.cyan;
            bubbleButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void UpdatePopulation() {
        population = Mathf.RoundToInt(populationSlider.value);
        populationText.text = population.ToString();
    }

}
