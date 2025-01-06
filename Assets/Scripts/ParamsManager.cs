using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ParamsManager : MonoBehaviour
{
    public SimulatorAI simulatorAI;
    public int ElitismCount = 10;
    public float MutationRate = 0.05f;

    private float biasV;
    private float biasH;
    private float[] weightsHDistances = new float[5];
    //adds
    private float[] weightsVDistances = new float[5];
    private float weightHSpeed;
    private float weightVSpeed;
    private int generation;


    public int PredefinedBias; 
    public int Generation => generation;

    
    void Start()
    {
        generation = 0;
    }

    
    void Update()
    {
        generation = simulatorAI.Generation;
    }

    public List<float> FindParameters(GameObject gameObject)
    {
        List<float> parameters = new List<float>();
        AIMovement aiMovement = gameObject.GetComponent<AIMovement>();
        float bH = aiMovement.BiasH;
        float bV = aiMovement.BiasV;
        float[] wHDistances = aiMovement.WeightsHDistances;
        //add
        float[] wVDistances = aiMovement.WeightsVDistances;
        float wHSpeed = aiMovement.WeightHSpeed;
        float wVSpeed = aiMovement.WeightVSpeed;

        parameters.Add(bH);
        parameters.Add(bV);
        parameters.AddRange(wHDistances);
        //add
        parameters.AddRange(wVDistances);
        parameters.Add(wHSpeed);
        parameters.Add(wVSpeed);

        return parameters;
    }

    public float MeasureFitness(GameObject gameObject)
    {
        AIMovement aiMovement = gameObject.GetComponent<AIMovement>();
        float fitness = aiMovement.Performance;
        return fitness;
    }

    public List<List<float>> GenerateNewPopulation(Dictionary<List<float>, float> sortedFitness, Dictionary<List<float>, float> elite)
    {
        List<List<float>> newPopulation = elite.Keys.ToList(); // Agregar los elites directamente

        while (newPopulation.Count < simulatorAI.Fitness.Count)
        {
            // Seleccionar padres únicamente de los elites
            List<float> parent1 = SelectParent(elite);
            List<float> parent2 = SelectParent(elite);

            List<float> offspring = Crossover(parent1, parent2);

            if (UnityEngine.Random.value < MutationRate)
            {
                Mutate(offspring);
            }

            newPopulation.Add(offspring);
        }

        return newPopulation;
    }


    List<float> SelectParent(Dictionary<List<float>, float> elite) // selección por ruleta dentro de los elites
    {
        float totalFitness = elite.Values.Sum();
        float randomValue = UnityEngine.Random.value * totalFitness;

        float cumulative = 0f;
        foreach (var pair in elite)
        {
            cumulative += pair.Value;
            if (cumulative >= randomValue)
            {
                return pair.Key;
            }
        }

        return elite.First().Key;
    }


    List<float> Crossover(List<float> parent1, List<float> parent2)
    {
        List<float> offspring = new List<float>();
        for (int i = 0; i < parent1.Count; i++)
        {
            offspring.Add(UnityEngine.Random.value < 0.5f ? parent1[i] : parent2[i]);
        }
        return offspring;
    }

    void Mutate(List<float> individual)
    {
        for (int i = 0; i < individual.Count; i++)
        {
            individual[i] += UnityEngine.Random.Range(-0.005f, 0.005f); // Ajuste de mutación
        }
    }


}
