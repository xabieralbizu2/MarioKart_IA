using System;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Rigidbody rb;
    public AIEnvironment aIEnvironment;

    public WheelCollider lfW, rfW, lbW, rbW;
    public Transform lfWTransform, rfWTransform, lbWTransform, rbWTransform;

    public float driveSpeed = 500f;
    public float steerSpeed = 60f;
    private float hInput, vInput;
    private bool isRunning = true;

    private float[] weightsHDistances = new float[5];
    private float[] weightsVDistances = new float[5];
    private float weightHSpeed = 0f;
    private float weightVSpeed = 0f;
    private float biasV = 0f;
    private float biasH = 0f;
    private Vector3 origin;

    private int performance = 0;
    private float timeElapsed = 0f;
    public float timeForDissapear = 80f;
    public event Action<GameObject> TimerDestrucion;

    // Getters
    public float Performance => performance;
    public float BiasV => biasV;
    public float BiasH => biasH;
    public float[] WeightsHDistances => weightsHDistances;
    public float[] WeightsVDistances => weightsVDistances;
    public float WeightHSpeed => weightHSpeed;
    public float WeightVSpeed => weightVSpeed;

    public void SetOrigin(Vector3 origin)
    {
        origin = origin;
    }

    public void SetWeightsDistances(float[] wHDistances, float[] wVDistances)
    {
        weightsHDistances = wHDistances;
        weightsVDistances = wVDistances;
        //add
    }

    public void SetWeightSpeed(float wHSpeed, float wVSpeed)
    {
        weightHSpeed = wHSpeed;
        weightVSpeed = wVSpeed;
        //add
    }

    public void SetBias(float bH, float bV)
    {
        biasH = bH;
        biasV = bV;
        //add
    }

    private (float, float) Check(float[] distances, float speed)
    {
        float VOutput = weightVSpeed * speed + biasV;
        float HOutput = weightHSpeed * speed + biasH;
        //add

        for (int i = 0; i < weightsHDistances.Length; i++)
        {
            VOutput += weightsVDistances[i] * distances[i];
            HOutput += weightsHDistances[i] * distances[i];
            //add
        }

        VOutput = Mathf.Clamp01(1f / (1f + Mathf.Exp(-VOutput))); // Limitamos entre 0 y 1
        HOutput = (float)Math.Tanh(HOutput);                     // Usar Math.Tanh y convertir a float
        //add
        //if (H1Output-H2Output)<threshold? 0: next line
        //if H1Output > H2Output? H1 output: -H2 output


        return (HOutput, VOutput);
    }


    private void FixedUpdate()
    {
        if (isRunning)
        {
            float[] distances = aIEnvironment.Distances;
            (hInput, vInput) = Check(distances, rb.linearVelocity.magnitude);

            // Movimiento hacia adelante y frenado
            float motorForce = vInput * driveSpeed;
            lfW.motorTorque = motorForce;
            rfW.motorTorque = motorForce;

            // Dirección (giro)
            float steerAngle = steerSpeed * hInput;
            lfW.steerAngle = steerAngle;
            rfW.steerAngle = steerAngle;

            // Actualizar posición y rotación de las ruedas
            UpdateWheel(lfW, lfWTransform);
            UpdateWheel(rfW, rfWTransform);
            UpdateWheel(lbW, lbWTransform);
            UpdateWheel(rbW, rbWTransform);

            CheckTime();
        }
    }

    private void UpdateWheel(WheelCollider collider, Transform transform)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        transform.position = position;
        transform.rotation = rotation;
    }

    private void CheckTime()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= timeForDissapear)
        {
            TimerDestructor(gameObject);
        }
    }

    private void TimerDestructor(GameObject car)
    {
        TimerDestrucion?.Invoke(car);
    }

    public void UpdatePerformance(int reward = 0)
    {
        performance += reward;
    }
}
