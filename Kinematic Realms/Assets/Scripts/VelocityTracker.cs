using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityTracker : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject. Please add one.", this);
            enabled = false; // Disable the script if no Rigidbody is found
        }
    }

    void FixedUpdate()
    {
        //rb.linearVelocity = new Vector3(0, 0, 0);

        Debug.Log("Horizontal Velocity: " + gameObject.transform.position.x + " m/s");
        Debug.Log("Vertical Velocity: " + gameObject.transform.position.y + " m/s");

        // Predict future position based on Rigidbody's current position and velocity
        PredictFuturePosition(1f);
    }

    void PredictFuturePosition(float secondsAhead)
    {
        Vector3 predictedPos = rb.position + rb.linearVelocity * secondsAhead;
        Debug.DrawLine(rb.position, predictedPos, Color.green);

        Debug.Log("Predicted Position in " + secondsAhead + "s: " + predictedPos.ToString("F2")); // "F2" formats to 2 decimal places
    }
}