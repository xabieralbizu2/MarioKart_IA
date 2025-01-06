using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulatorAI : MonoBehaviour
{

    private Dictionary<List<float>, float> fitness = new Dictionary<List<float>, float>();
    private Dictionary<List<float>, float> bestOfExp = new Dictionary<List<float>, float>();
    private List<List<float>> newGeneration;

    private float elapsedTime = 0f;
    private int maxGeneration = 15;
    private int generation = 0;

    public AICreator aiCreator;
    public TimeManager timeManager;
    public ParamsManager paramsManager;


    public int Generation => generation;
    public List<List<float>> NewGeneration => newGeneration;
    public Dictionary<List<float>, float> Fitness => fitness;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > timeManager.timeForDissapear + timeManager.timeBtwGenerations)
        {
            Dictionary <List<float>, float> sortedFitness = RankCars();
            var elite = sortedFitness.Take(10).ToDictionary(pair => pair.Key, pair => pair.Value); //sustituir 10 por paramsManager.Elitism

            newGeneration = paramsManager.GenerateNewPopulation(sortedFitness, elite);


            fitness.Clear();
            foreach (var paramsSet in newGeneration)
            {
                fitness[paramsSet] = 0f; // Inicializamos el fitness en 0
            }


            generation++;
            Debug.Log(generation);

            aiCreator.ResetI();
            elapsedTime = 0f;

        }
        elapsedTime += Time.deltaTime;

    }

    Dictionary<List<float>, float> RankCars()
    {
        var sortedFitness = fitness.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        return sortedFitness;
    }
    public void HandleWallCollision(GameObject gameObject)
    {
        float fitness_msr = paramsManager.MeasureFitness(gameObject);
        List<float> parameters = paramsManager.FindParameters(gameObject);
        fitness.Add(parameters, fitness_msr);


    }




   


}

