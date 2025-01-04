using System;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    public event Action<GameObject> objectColliderDestruction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        GameObject collidingObject = collision.gameObject;
        ObjectColliderDestructor(collidingObject);
    }
    public void ObjectColliderDestructor(GameObject gameObject)
    {
        objectColliderDestruction?.Invoke(gameObject);
    }
}
