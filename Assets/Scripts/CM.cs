using System;
using UnityEngine;


public class CarController : MonoBehaviour
{
    public Rigidbody rb;


    public WheelCollider lfW, rfW, lbW, rbW;
    public float driveSpeed, steerSpeed;
    float hInput, vInput;
    const float mult = 3;
    bool isRunning;



    //Update
    (float, float) Check()
    {
        float houtput;
        float voutput;
        houtput = Input.GetAxis("Horizontal");
        voutput = Input.GetAxis("Vertical");
        return (houtput, voutput);
    }

    void Start()
    {
        isRunning = true;
    }

    private void Update()
    {
        if (isRunning)
        {

            (hInput, vInput) = Check();
            Debug.Log(hInput);
            Debug.Log(vInput);
            ForwardMovement(vInput);
            SideWaysMovement(hInput);
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
        lbW.steerAngle = steerSpeed * hInput;
        rbW.steerAngle = steerSpeed * hInput;
    }

    private void Accelerate(float motor)
    {
        lfW.motorTorque = motor;
        rfW.motorTorque = motor;
        lbW.motorTorque = motor;
        rbW.motorTorque = motor;

        // Desactiva el frenado mientras hay movimiento
        lfW.brakeTorque = 0;
        rfW.brakeTorque = 0;
        lbW.brakeTorque = 0;
        rbW.brakeTorque = 0;
    }

    private void Stop(float motor)
    {
        float brakeTorque = Mathf.Abs(motor);
        lfW.brakeTorque = brakeTorque;
        rfW.brakeTorque = brakeTorque;
        lbW.brakeTorque = brakeTorque;
        rbW.brakeTorque = brakeTorque;

        // Detén el motor torque para evitar conflictos
        lfW.motorTorque = 0;
        rfW.motorTorque = 0;
        lbW.motorTorque = 0;
        rbW.motorTorque = 0;
    }


}