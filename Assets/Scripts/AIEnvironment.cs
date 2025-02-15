using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnvironment : MonoBehaviour
{

    private float[] distances;
    private RaycastHit raycastHit;
    int layerMask = 1;
    public float[] Distances
    {
        get{ return distances; }
    }


    float[] ComputeDistances()
    {
        // Usar TransformDirection para convertir las direcciones relativas a coordenadas globales
        Ray ray_f = new Ray(transform.position + new Vector3(0,0.5f,0), transform.TransformDirection(Vector3.forward));
        Ray ray_l = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.left));
        Ray ray_r = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.right));
        Ray ray_fl = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward - Vector3.right));
        Ray ray_fr = new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward + Vector3.right));

        float distance_l = CheckDistance(ray_l);
        float distance_fl = CheckDistance(ray_fl);
        float distance_f = CheckDistance(ray_f);
        float distance_fr = CheckDistance(ray_fr);
        float distance_r = CheckDistance(ray_r);

        return new float[] { distance_l, distance_fl, distance_f, distance_fr, distance_r };
    }

    float CheckDistance(Ray ray)
    {
        float distance;
        if (Physics.Raycast(transform.position,ray.direction, out raycastHit, 300f, layerMask))
        {
            distance = raycastHit.distance;

        }
        else
        {
            distance = 300f;

        }
        Debug.DrawLine(transform.position, raycastHit.point, Color.red);
        return distance;
    }

    // Update is called once per frame
    void Update()
    {
        distances = ComputeDistances();
    }


}
