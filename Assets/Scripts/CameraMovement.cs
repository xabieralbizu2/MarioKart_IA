using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform carTransform;
    enum State
    {
        state_running
    };
    private State state_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state_ = State.state_running;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state_)
        {
            case State.state_running:
                transform.position = carTransform.position;
                transform.rotation = carTransform.rotation;
                break;
        }

        
    }
}
