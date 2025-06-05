using System.Collections.Generic;
using UnityEngine;

public class VelocityGraphUI : MonoBehaviour
{
    public Transform target; // Object to track
    public float graphDuration = 5f; // How many seconds to show on X-axis
    public float yScale = 1f; // Scale of Y axis (m/s)
    public float xScale = 50f; // Pixels per second
    public float updateRate = 0.1f; // Time between samples

    private List<float> velocitySamples = new List<float>();
    private List<float> timeSamples = new List<float>();
    private Vector3 lastPosition;
    private float timeElapsed;
    private float updateTimer;

    private LineRenderer lineRenderer;

    void Start()
    {
        lastPosition = target.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        updateTimer += Time.deltaTime;

        // Sample velocity every updateRate seconds
        if (updateTimer >= updateRate)
        {
            float velocity = (target.position - lastPosition).magnitude / updateTimer;
            velocitySamples.Add(velocity);
            timeSamples.Add(timeElapsed);

            lastPosition = target.position;
            updateTimer = 0f;

            // Trim old samples beyond graphDuration
            while (timeSamples.Count > 0 && timeElapsed - timeSamples[0] > graphDuration)
            {
                timeSamples.RemoveAt(0);
                velocitySamples.RemoveAt(0);
            }

            DrawGraph();

        }
    }

    void DrawGraph()
    {
        int count = velocitySamples.Count;
        lineRenderer.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            float x = (timeSamples[i] - timeSamples[0]) * xScale;
            float y = velocitySamples[i] * yScale;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
