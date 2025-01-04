using System;
using UnityEngine;


public class AIMovement : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject gameObject;
    public AIEnvironment aIEnvironment;

    public WheelCollider lfW, rfW, lbW, rbW;
    public float driveSpeed, steerSpeed;
    float hInput, vInput;
    const float mult=3;
    bool isRunning;

    private float[] weightsHDistances = new float[5];
    private float[] weightsVDistances = new float[5];
    private float weightHSpeed = 0f;
    private float weightVSpeed = 0f;
    private float biasV = 0f;
    private float biasH = 0f;

    private int performance = 0;
    private Vector3 origin;

    public float Performance => performance;
    public float BiasV => biasV;
    public float BiasH => biasH;
    public float[] WeightsHDistances => weightsHDistances;
    public float[] WeightsVDistances => weightsVDistances;
    public float WeightHSpeed => weightHSpeed;
    public float WeightVSpeed => weightVSpeed;

    private float timeElapsed = 0f;
    public float timeForDissapear = 180f;
    public event Action<GameObject> TimerDestrucion;

    public void SetOrigin(Vector3 origin)
    {
        origin = origin;
    }

    public void SetWeightsDistances(float[] wHDistances, float[] wVDistances)
    {
        weightsHDistances = wHDistances;
        weightsVDistances = wVDistances;
    }

    public void SetWeightSpeed(float wHSpeed, float wVSpeed)
    {
        weightHSpeed = wHSpeed;
        weightVSpeed = wVSpeed;
    }

    public void SetBias(float bH, float bV)
    {
        biasV = bV;
        biasH = bH;
    }

    //Update
    (float, float) Check(float[] distances, float speed)
    {
        float VOutput = weightVSpeed * speed + biasV;
        float HOutput = weightHSpeed * speed + biasH;
        for (int i= 0; i<weightsHDistances.Length; i++)
        {
            VOutput += weightsVDistances[i] * distances[i];
            HOutput += weightsHDistances[i] * distances[i];
        }
        //salidas entre 0 y 1 / -1 y 1

        VOutput = 1f / (1f + Mathf.Exp(-VOutput)); 
        HOutput = MathF.Tanh(HOutput);


        return (HOutput, VOutput);
    }

    void Start()
    {
        isRunning = true;
    }

    private void Update()
    {
        if (isRunning)
        {
            float[] distances = aIEnvironment.Distances;
            (hInput, vInput) = Check(distances, rb.linearVelocity.magnitude);
            ForwardMovement(vInput);
            SideWaysMovement(hInput);
            CheckTime();
        }
    }



    //Movement
    private void ForwardMovement(float vInput)
    {
        float motor = vInput * driveSpeed;
        float brake = driveSpeed * mult;

        if (vInput != 0) 
        {
            Accelerate(motor);
        }
        else 
        {
            Stop(brake);
        }
    }
    private void SideWaysMovement(float hInput)
    {
        lfW.steerAngle = steerSpeed * hInput;
        rfW.steerAngle = steerSpeed * hInput;
    }

    private void Accelerate(float motor)
    {
        lfW.motorTorque = motor ;
        rfW.motorTorque = motor ;
        lbW.motorTorque = motor ;
        rbW.motorTorque = motor ;

        // Desactiva el frenado mientras hay movimiento
        lfW.brakeTorque = 0;
        rfW.brakeTorque = 0;
        lbW.brakeTorque = 0;
        rbW.brakeTorque = 0;
    }

    private void Stop(float motor)
    {
        float brakeTorque = Mathf.Abs(motor);
        lfW.brakeTorque = brakeTorque ;
        rfW.brakeTorque = brakeTorque ;
        lbW.brakeTorque = brakeTorque ;
        rbW.brakeTorque = brakeTorque ;

        // Detén el motor torque para evitar conflictos
        lfW.motorTorque = 0;
        rfW.motorTorque = 0;
        lbW.motorTorque = 0;
        rbW.motorTorque = 0;
    }

    public void UpdatePerformance(int reward=0)
    {
        performance += reward;
    }

    float DistanceBetweenPoints(Vector3 pointA, Vector3 pointB)
    {
        return Vector3.Distance(pointA, pointB);
    }

    void TimerDestructor(GameObject gameObject)
    {
        TimerDestrucion?.Invoke(gameObject);
    }
    void CheckTime()
    {
        timeElapsed += Time.deltaTime;  

        if (timeElapsed >= timeForDissapear)  
        {
            GameObject container = rb.gameObject;
            TimerDestructor(container);
        }
    }
}