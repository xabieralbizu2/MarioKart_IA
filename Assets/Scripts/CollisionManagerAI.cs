using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManagerAI : MonoBehaviour
{
    public string tag1 = "Wall";
    public string tag2 = "Player";
    private bool collision;

    void Start()
    {

        IgnoreContacts("Ignore Raycast");
    }
    void Update()
    {
    }

    void IgnoreContacts(string layer_str)
    {
        int layer = LayerMask.NameToLayer(layer_str);
        Physics.IgnoreLayerCollision(layer, layer, true);
    }

    public void HandleWallCollision(GameObject gameObject)
    {
        AIMovement aIMovement = gameObject.GetComponent<AIMovement>();
        aIMovement.UpdatePerformance();
        Destroy(gameObject);
    }
}


