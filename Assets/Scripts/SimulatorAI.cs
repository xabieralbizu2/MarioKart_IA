using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulatorAI : MonoBehaviour
{

    private Dictionary<List<float>, float> fitness = new Dictionary<List<float>, float>();
    private Dictionary<List<float>, float> bestOfExp = new Dictionary<List<float>, float>();
    private List<float> bestParams;


    private int maxGeneration = 15;
    
    public AICreator aiCreator;
    public TimeManager timeManager;

    private int generation = 0;
    private float elapsedTime = 0f;
    private float bestFitness = 0f;

    public int Generation => generation;
    public List<float> BestParams => bestParams;
    public float BestFitness => bestFitness;

    public ParamsManager paramsManager;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > timeManager.timeForDissapear + timeManager.timeBtwGenerations)
        {
            Dictionary <List<float>, float> sortedFitness = RankCars();
            var varBestOfRound = sortedFitness.First();
       
            Dictionary<List<float>, float> bestOfRound = new Dictionary<List<float>, float> { { varBestOfRound.Key, varBestOfRound.Value } };
            (bestParams, bestFitness) = paramsManager.ChooseBest(bestOfExp, bestOfRound);
            Debug.Log(bestFitness);
            foreach (float param in bestParams)
            {
                Debug.Log(param);
            }
            bestOfExp = new Dictionary<List<float>, float> { { bestParams, bestFitness } };

            generation++;

            aiCreator.ResetI();
            elapsedTime = 0f;
            Debug.Log(generation);

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

