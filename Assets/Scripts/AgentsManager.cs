using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    AIMovement aIMovement;
    private List<GameObject> agents = new List<GameObject>();
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void AddAgent(GameObject gameObject)
    {
        agents.Add(gameObject);
        aIMovement = gameObject.GetComponent<AIMovement>();
        aIMovement.TimerDestrucion += HandleTime;
    }
    void HandleTime(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
