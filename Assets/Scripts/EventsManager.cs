using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public CollisionManagerAI collisionManager;
    public SimulatorAI simulatorAI;
    private WallCollider[] walls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        walls = FindWallColliders();
        Debug.Log(walls.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    WallCollider[] FindWallColliders()
    {
        WallCollider[] wallArray = FindObjectsOfType<WallCollider>();
        foreach (WallCollider wall in wallArray)
        {
            wall.objectColliderDestruction += HandleEvent;
        }
        return wallArray;
    }
    void HandleEvent(GameObject gameObject)
    {
        simulatorAI.HandleWallCollision(gameObject);
        collisionManager.HandleWallCollision(gameObject);

    }
}
