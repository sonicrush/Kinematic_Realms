using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float forceAmount = 10f; // Force applied for movement
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("Hello world");
    }

    private void Update()
    {

        // Movement controls
        // Check if the user is holding down the left mouse button on this frame.

        if (Input.GetMouseButton(0))
        {
            Debug.Log("The left mouse button is being held down.");
        }

        // Right mouse button
        if (Input.GetMouseButton(1))
        {
            Debug.Log("The right mouse button is being held down.");
        }

        // Middle mouse button
        if (Input.GetMouseButton(2)) 
        {
            Debug.Log("The middle mouse button is being held down.");
        }

    }

}
