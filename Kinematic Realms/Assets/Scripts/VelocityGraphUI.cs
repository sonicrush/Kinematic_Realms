using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityGraphUI : MonoBehaviour
{
    public Transform target; // Object to track
    public float graphDuration = 5f; // How many seconds to show on X-axis (e.g., 5 seconds)

    [Header("Graph World Dimensions")]
    public float graphWidth = 1f; // Desired width of graph in world units
    public float graphHeight = 0.5f; // Desired height of graph in world units

    [Header("Velocity Scaling")]
    public float maxVelocityToShow = 10f; // The max velocity that will reach the top of the graph (m/s)

    [Header("Line Properties")]
    public float lineWidth = 0.1f; // Public variable to easily adjust line thickness in Inspector

    [Header("Axis Labels")]
    public TextMeshProUGUI yValueLabelPrefab; // Y_Value_Label_Prefab goes here
    public int numberOfYLabels = 5; // How many Y-axis value labels to show (e.g., 0, 5, 10)
    public float yLabelOffset = 0.1f; // How far left of the graph Y labels should be
    public float yLabelSpacingFactor = 1.0f; // Control vertical spacing between labels


    public float updateRate = 0.1f; // Time between samples

    private List<float> velocitySamples = new List<float>();
    private List<float> timeSamples = new List<float>();
    private Vector3 lastPosition;
    private float timeElapsed;
    private float updateTimer;

    private LineRenderer lineRenderer;
    private List<TextMeshProUGUI> yLabels = new List<TextMeshProUGUI>();

    void Start()
    {
        lastPosition = target.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        // Ensure the line renderer is using local space for positions
        lineRenderer.useWorldSpace = false;

        // Controls line thickness
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Initialize Y-axis labels
        SetupYAxisLabels();

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
            UpdateYAxisLabels(); // Update positions and values

        }
    }

    void DrawGraph()
    {
        int count = velocitySamples.Count;
        lineRenderer.positionCount = count;

        float actualXScale = graphWidth / graphDuration;
        float actualYScale = graphHeight / maxVelocityToShow;

        for (int i = 0; i < count; i++)
        {
            float x = (timeSamples[i] - timeSamples[0]) * graphWidth;
            float y = velocitySamples[i] * graphHeight;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void SetupYAxisLabels()
    {
        if (yValueLabelPrefab == null)
        {
            Debug.LogWarning("Y Value Label Prefab is not assigned to VelocityGraphUI script.");
            return;
        }

        // Clear existing labels if any (e.g., if re-running Start in editor)
        foreach (var label in yLabels)
        {
            Destroy(label.gameObject);
        }
        yLabels.Clear();

        // Instantiate and position Y-axis value labels
        for (int i = 0; i < numberOfYLabels; i++)
        {
            TextMeshProUGUI newLabel = Instantiate(yValueLabelPrefab, lineRenderer.transform.parent); // Parent to the Canvas
            newLabel.gameObject.SetActive(true);
            yLabels.Add(newLabel);
        }
        UpdateYAxisLabels(); // Initial positioning
    }

    void UpdateYAxisLabels()
    {
        if (yLabels.Count == 0 || maxVelocityToShow <= 0) return;

        Vector3 graphBottomLeftWorld = transform.position;

        // Calculate the effective vertical step size for labels
        // We divide graphHeight by (numberOfYLabels - 1) to get even spacing,
        // then multiply by yLabelSpacingFactor to adjust that spacing.
        float effectiveStepHeight = (numberOfYLabels > 1) ? (graphHeight / (numberOfYLabels - 1)) * yLabelSpacingFactor : 0;

        // Calculate the velocity increment for each label
        float velocityIncrement = (numberOfYLabels > 1) ? maxVelocityToShow / (numberOfYLabels - 1) : maxVelocityToShow / 2; // For single label, center it at half max velocity

        for (int i = 0; i < numberOfYLabels; i++)
        {
            float targetVelocity = i * velocityIncrement;

            // Calculate y-position based on the effective step height
            float yWorldPosition = graphBottomLeftWorld.y + (i * effectiveStepHeight);

            // If you want the labels to only go up to maxVelocityToShow, and potentially be more spread out
            // You might want to use:
            // float yWorldPosition = graphBottomLeftWorld.y + (targetVelocity / maxVelocityToShow) * graphHeight * yLabelSpacingFactor;
            // The current setup (i * effectiveStepHeight) is more direct for physical spacing.

            float xWorldPosition = graphBottomLeftWorld.x - yLabelOffset;

            yLabels[i].rectTransform.position = new Vector3(xWorldPosition, yWorldPosition, graphBottomLeftWorld.z);
            yLabels[i].text = targetVelocity.ToString("F1"); // Format to one decimal place
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;

        if (Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Vector3 topLeft = transform.position + new Vector3(0, graphHeight, 0);
        Vector3 topRight = transform.position + new Vector3(graphWidth, graphHeight, 0);
        Vector3 bottomLeft = transform.position + new Vector3(0, 0, 0);
        Vector3 bottomRight = transform.position + new Vector3(graphWidth, 0, 0);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, new Vector3(bottomLeft.x, bottomLeft.y + maxVelocityToShow * (graphHeight / maxVelocityToShow), bottomLeft.z));

    }

}
