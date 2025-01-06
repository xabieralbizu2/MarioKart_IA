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


    private float biasV;
    private float biasH;
    private float[] weightsHDistances = new float[5];
    //add
    private float[] weightsVDistances = new float[5];
    private float weightHSpeed;
    private float weightVSpeed;
    private int predefinedBias;

    private float nextUpdateTime;
    private int i;
    private List<float> bestParams;
    private float bestFitness;
    private float maxFitness = 1314 / 2;


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
            if (generation == 0)
            {
                SetWeightsInstance(aIMovement, new List<float>(), generation);
            }
            else
            {
                SetWeightsInstance(aIMovement, simulatorAI.NewGeneration[i], generation);
            }

            i++;

        }


    }


    float DefineRandomV()
    {
        float u1 = UnityEngine.Random.value; // Primer número aleatorio entre 0 y 1
        float u2 = UnityEngine.Random.value; // Segundo número aleatorio entre 0 y 1

        // Generar un valor de una distribución normal estándar (media=0, desviación estándar=1)
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        // Escalar al rango deseado [-0.005, 0.005]
        float scale = 0.005f;
        return randStdNormal * scale;
    }

    GameObject InstantiatePrefab()
    {

        Quaternion rotation = Quaternion.Euler(0f, 110f, 0f);
        AIMovement aIMovement = carPrefab.GetComponent<AIMovement>();
        GameObject instance = Instantiate(carPrefab, spawnArea, rotation);

        aIMovement.SetOrigin(spawnArea);

        return instance;
    }


    void RegisterAgent(GameObject gameObject)
    {
        agentsManager.AddAgent(gameObject);
    }


    void SetWeightsInstance(AIMovement aIMovement, List<float> paramsSet, int generation)
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
            biasH = paramsSet[0];
            biasV = paramsSet[1];
            weightHSpeed = paramsSet[12];
            weightVSpeed = paramsSet[13];

            for (int j = 0; j < 5; j++)
            {
                weightsHDistances[j] = paramsSet[2 + j];
                weightsVDistances[j] = paramsSet[7 + j];
            }

            aIMovement.SetWeightSpeed(weightHSpeed, weightVSpeed);
            aIMovement.SetWeightsDistances(weightsHDistances, weightsVDistances);
            aIMovement.SetBias(biasH, biasV);
        }
    }


}
