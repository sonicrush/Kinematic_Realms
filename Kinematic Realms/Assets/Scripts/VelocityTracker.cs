using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    private Vector3 lastPosition;
    public Vector3 currentVelocity { get; private set; }

    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate velocity: change in position over time
        currentVelocity = (transform.position - lastPosition) / Time.deltaTime;

        //Log velocity in meters/second
        Debug.Log("Velocity: " + currentVelocity + " m/s");

        //Update last position for the next frame
        lastPosition = transform.position;

        PredictFuturePosition(1f);

    }

    void PredictFuturePosition(float secondsAhead)
    {
        Vector3 predictedPos = transform.position + currentVelocity * secondsAhead;
        Debug.DrawLine(transform.position, predictedPos, Color.green);

        Debug.Log("Predicted Position in " + secondsAhead + "s: " + predictedPos);
    }

}
