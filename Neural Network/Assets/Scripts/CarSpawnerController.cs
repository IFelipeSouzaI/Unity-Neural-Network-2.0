using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerController : MonoBehaviour
{
    [Header("DEV Options")]
    public GameObject carPrefab;
    public List<GameObject> cars;

    private GeneticAlgorithm GA;

    public int currentCount = 0;

    [Header("GA Options")]
    public int populationSize = 2;
    public float timeToRestart = 15f;

    private void Start(){
        GA = new GeneticAlgorithm();
        for(int i = 0; i < populationSize; i++) {
            var car = Instantiate(carPrefab, transform.position, Quaternion.identity);
            car.transform.parent = transform;
            cars.Add(car);
        }
        for(int i = 0; i < populationSize; i++) {
            GA.AddToDNAList(cars[i].GetComponent<CarMovement>().GetDNA());
        }
    }

    private void Update() {
        if(currentCount >= cars.Count) {
            GA.Process();
            cars[GA.bestFitnessIndex].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            cars[GA.bestFitnessIndex].transform.localScale = new Vector3(2,2,2);
            currentCount = 0;
            Invoke("Restart", 2f);
        }
    }

    private void Restart() {
        cars[GA.bestFitnessIndex].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        cars[GA.bestFitnessIndex].transform.localScale = new Vector3(1,1,1);
        for(int i = 0; i < populationSize;i++) {
            cars[i].GetComponent<CarMovement>().ResetToStart();
            cars[i].GetComponent<CarMovement>().SetDNA(GA.GetDNAFromIndex(i));
        }
    }

}
