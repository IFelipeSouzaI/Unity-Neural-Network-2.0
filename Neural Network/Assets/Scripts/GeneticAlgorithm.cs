using System.Collections.Generic;
using UnityEngine;
public class GeneticAlgorithm
{
    //private int populationAmount = 300;

    public GameObject carPrefab;
    private List<DNA> population = new List<DNA>();
    private float bestFitness = 0;
    private float fitnessTotal = 0;

    // Debug
    private int loopCount = 0;
    public int bestFitnessIndex = 0;

    // Alpha+-
    private int currentAlphaIndex = 0;
    private float randomAlphaValue = 0;

    public GeneticAlgorithm() {
    }

    public void AddToDNAList(DNA newDNA) {
        population.Add(newDNA);
    }

    public void CalculateFitness() {
        fitnessTotal = 0;
        bestFitness = 0;
        for(int i = 0; i < population.Count; i++) {
            float currentFitness = population[i].GetFitness();
            if(currentFitness > bestFitness) {
                bestFitness = currentFitness;
                bestFitnessIndex = i;
            }
            fitnessTotal += currentFitness;
        }
        Debug.Log(bestFitnessIndex);
        foreach(DNA dna in population) {
            dna.FitnessNormalize(/*bestFitness*/fitnessTotal);
        }
    }

    public DNA GetDNAFromIndex(int index) {
        return population[index];
    }

    public int GetOneAlphaIndex() {
        currentAlphaIndex = -1;
        randomAlphaValue = Random.Range(0f, 1f);
        while(randomAlphaValue > 0f) {
            currentAlphaIndex += 1;
            if(currentAlphaIndex >= population.Count) {
                return Random.Range(0, population.Count);
            }
            randomAlphaValue -= population[currentAlphaIndex].fitness;
        }
        return currentAlphaIndex;
    }

    public void CreateNextPopulation() {
        List<DNA> newPopulation = new List<DNA>();
        for(int i = 0; i < population.Count; i++) {
            DNA father = population[GetOneAlphaIndex()];
            DNA mother = population[GetOneAlphaIndex()];
            DNA child = new DNA(father.mutationRate);
            child.FillDnaFromParents(father, mother, 1);
            newPopulation.Add(child);
        }
        population.Clear();
        population = newPopulation;
    }

    public void Process() {
        loopCount += 1;
        CalculateFitness();
        /*Debug.Log("Best Fit: " + bestFitness + " - Best Fit Index: " + bestFitnessIndex);*/
        Debug.Log("Loop Count: " + loopCount);
        CreateNextPopulation();
    }

}