using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ParamsManager : MonoBehaviour
{
    public SimulatorAI simulatorAI;

    private float biasV;
    private float biasH;
    private float[] weightsHDistances = new float[5];
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
        float[] wVDistances = aiMovement.WeightsVDistances;
        float wHSpeed = aiMovement.WeightHSpeed;
        float wVSpeed = aiMovement.WeightVSpeed;

        parameters.Add(bH);
        parameters.Add(bV);
        parameters.AddRange(wHDistances);
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

    public (List<float>, float) ChooseBest(Dictionary<List<float>, float> item1, Dictionary<List<float>, float> item2)
    {
        List<float> bestParams = new List<float>();
        float bestFitness = 0f;
        var item2Best = item2.First();
        if (item1.Any() && item2.Any())
        {
            var item1Best = item1.First();
            if (item1Best.Value > item2Best.Value)
            {
                bestParams = item1Best.Key;
                bestFitness = item1Best.Value;
            }
            else
            {
                bestParams = item2Best.Key;
                bestFitness = item2Best.Value;
            }
        }
        else
        {
            bestParams = item2Best.Key;
            bestFitness = item2Best.Value;
        }
        return (bestParams, bestFitness);
    }


}
