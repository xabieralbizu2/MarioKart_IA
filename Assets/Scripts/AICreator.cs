using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICreator : MonoBehaviour
{
    public GameObject carPrefab;
    public AgentsManager agentsManager;
    public SimulatorAI simulatorAI;
    public ParamsManager paramsManager;
    public float interval = 1f;
    public int numCars;
    public Vector3 spawnArea;
    public float alpha;

    private float biasV;
    private float biasH;
    private float[] weightsHDistances = new float[5];
    private float[] weightsVDistances = new float[5];
    private float weightHSpeed;
    private float weightVSpeed;
    private int predefinedBias;

    private float nextUpdateTime;
    private int i;
    private List<float> bestParams;

    public int NumCars => numCars;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        nextUpdateTime = 0f;
        predefinedBias = paramsManager.PredefinedBias;

    }

    public void ResetI()
    {
        i = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            bestParams = simulatorAI.BestParams;
            PerformUpdate();
            nextUpdateTime = Time.time + interval;
            
        }
    }
    void PerformUpdate()
    {
        if (i < numCars)
        {
            int generation = simulatorAI.Generation;

            GameObject instance = InstantiatePrefab();
            RegisterAgent(instance);

            AIMovement aIMovement = instance.GetComponent<AIMovement>();
            SetWeightsInstance(aIMovement, bestParams, generation);
            i++;

        }


    }


    float DefineRandomV()
    {
        int rand = UnityEngine.Random.Range(-50, 51); // Rango de -5 a 5
        return rand * 0.00005f; // Rango entre -0.0025 y 0.0025 con incrementos de 0.0005
    }
    GameObject InstantiatePrefab()
    {
        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(spawnArea.x, spawnArea.x),
            spawnArea.y+2f,
            UnityEngine.Random.Range(spawnArea.z, spawnArea.z)
        );
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        AIMovement aIMovement = carPrefab.GetComponent<AIMovement>();
        GameObject instance = Instantiate(carPrefab, randomPosition, rotation);

        aIMovement.SetOrigin(spawnArea);

        return instance;
    }


    void RegisterAgent(GameObject gameObject)
    {
        agentsManager.AddAgent(gameObject);
    }

    float ChooseParam(float paramFather, float paramRandom, int generation )
    {


        float param = (UnityEngine.Random.value < alpha) ? paramFather : paramRandom;
        return param;
    }

    void SetWeightsInstance(AIMovement aIMovement, List<float> bestParams, int generation)
    {
        if (generation == 0)
        {
            biasH = DefineRandomV() * predefinedBias;
            biasV = DefineRandomV() * predefinedBias;
            weightHSpeed = DefineRandomV();
            weightVSpeed = DefineRandomV();
            for (int j = 0; j < 5; j++)
            {
                weightsHDistances[j] = DefineRandomV();
                weightsVDistances[j] = DefineRandomV();
            }
            aIMovement.SetWeightSpeed(weightHSpeed, weightVSpeed);
            aIMovement.SetWeightsDistances(weightsHDistances, weightsVDistances);
            aIMovement.SetBias(biasH, biasV);
        }
        else
        {
            biasH = ChooseParam(bestParams[0], DefineRandomV() * predefinedBias,generation);
            biasV = ChooseParam(bestParams[1], DefineRandomV() * predefinedBias,generation);
            weightHSpeed = ChooseParam(bestParams[12], DefineRandomV(),generation);
            weightVSpeed = ChooseParam(bestParams[13], DefineRandomV(),generation);
            for (int j = 0; j < 5; j++)
            {
                weightsHDistances[j] = ChooseParam(bestParams[2 + j], DefineRandomV(),generation);
                weightsVDistances[j] = ChooseParam(bestParams[7 + j], DefineRandomV(),generation);
            }
            aIMovement.SetWeightSpeed(weightHSpeed, weightVSpeed);
            aIMovement.SetWeightsDistances(weightsHDistances, weightsVDistances);
            aIMovement.SetBias(biasH, biasV);
        }
    }


}
