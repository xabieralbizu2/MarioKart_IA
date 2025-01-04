using System;
using UnityEngine;

public class WM : MonoBehaviour
{
    public WheelCollider wheel;
    public Transform parent; // Change type to MeshRenderer
    public bool wheelTurn;

    // Update is called once per frame
    void Update()
    {
        if (wheelTurn)
        {
            parent.localEulerAngles = new Vector3(
                parent.localEulerAngles.x,
                wheel.steerAngle,
                parent.localEulerAngles.z
            );
        }

        float rotation = wheel.rpm / 60 * 360 * Time.deltaTime;

        // Evitar rotación inválida
        if (!float.IsNaN(rotation) && rotation != Mathf.Infinity)
        {
            parent.Rotate(rotation, 0, 0);
        }
    }
}
