using TMPro;
using UnityEngine;

public class TotalEnergy : MonoBehaviour
{
    public float totalEnergy = 0f;
    public float height = 0f;
    public float velocity = 0f;
    public float referenceHeight = 0f; // Set this in inspector if needed
    public bool useReferenceHeight = false; // Toggle between ground or reference

    private Rigidbody rb;
    private float gravity = 9.81f; // Earth's gravity

    public TextMeshProUGUI energyText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Kinetic Energy
        velocity = rb.linearVelocity.magnitude;
        float kineticEnergy = 0.5f * rb.mass * velocity * velocity;

        // Height for Potential Energy
        if (useReferenceHeight)
        {
            height = transform.position.y - referenceHeight;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                height = hit.distance;
            }
        }

        float potentialEnergy = rb.mass * gravity * Mathf.Max(height, 0); // Avoid negative PE

        // Total Energy
        totalEnergy = potentialEnergy + kineticEnergy;

        if (energyText != null)
        {
            energyText.text = $"Total Energy: {totalEnergy:F2} J";
        }
    }
}