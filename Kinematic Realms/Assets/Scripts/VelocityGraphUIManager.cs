using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityGraphUIManager : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public RectTransform graphRect; // This should be the RectTransform of the graph image
    public UIVelocityGraphRenderer graphRenderer;
    public TextMeshProUGUI xLabelPrefab;
    public TextMeshProUGUI yLabelPrefab;
    public RectTransform xLabelParent;
    public RectTransform yLabelParent;
    public GameObject GraphContainer;

    [Header("Graph Settings")]
    public float graphDuration = 5f;
    public float maxVelocityToShow = 10f;
    public float updateRate = 0.1f;

    [Header("Y Axis Labels")]
    public int numberOfYLabels = 5;
    public float yLabelSpacingFactor = 1.0f; // Control vertical spacing between labels
    public float yLabelOffset = -40f; // Default offset to left of graph

    [Header("X Axis Labels")]
    public int numberOfXLabels = 6;
    public float xLabelSpacingFactor = 1.0f; // Multiply horizontal spacing for labels
    public float xLabelOffset = -20f; // Default offset below the graph
    public float xLabelHorizontalOffset = 0f;

    private List<float> timeSamples = new();
    private List<float> velocitySamples = new();
    private float timeElapsed = 0f;
    private float updateTimer = 0f;
    private Vector3 lastPosition;

    private List<TextMeshProUGUI> xLabels = new();
    private List<TextMeshProUGUI> yLabels = new();

    void Start()
    {
        lastPosition = target.position;

        SetupYLabels();
        SetupXLabels();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        updateTimer += Time.deltaTime;

        if (updateTimer >= updateRate)
        {
            float velocity = (target.position - lastPosition).magnitude / updateTimer;
            velocitySamples.Add(velocity);
            timeSamples.Add(timeElapsed);

            lastPosition = target.position;
            updateTimer = 0f;

            while (timeSamples.Count > 0 && timeElapsed - timeSamples[0] > graphDuration)
            {
                timeSamples.RemoveAt(0);
                velocitySamples.RemoveAt(0);
            }

            UpdateGraph();
            UpdateXLabels();
            UpdateYLabels();
        }
    }

    void UpdateGraph()
    {
        Rect rect = graphRenderer.GetComponent<RectTransform>().rect;
        RectTransform GraphContainerRect = GraphContainer.GetComponent<RectTransform>();

        float width = rect.width;
        float height = rect.height;
        float xScale = width / graphDuration;
        float yScale = height / maxVelocityToShow;

        graphRenderer.graphPoints.Clear();

        if (timeSamples.Count == 0 || velocitySamples.Count == 0)
            return;

        float baseTime = timeSamples[0];

        for (int i = 0; i < velocitySamples.Count; i++)
        {
            if (i >= timeSamples.Count)
                break;

            float timeOffset = timeSamples[i] - baseTime;
            float x = timeOffset * xScale + GraphContainerRect.position.x;
            float y = velocitySamples[i] * yScale + GraphContainerRect.position.y;

            // Ensure y is clamped inside the graph height
            y = Mathf.Clamp(y, 0f, height);

            graphRenderer.graphPoints.Add(new Vector2(x, y));
        }

        graphRenderer.Redraw();
    }

    private void OnDrawGizmos()
    {
        Rect rect = GetComponent<RectTransform>().rect;
        Vector3[] corners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(corners);

        Gizmos.color = Color.red;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
        }
    }

    void SetupYLabels()
    {
        float height = graphRect.rect.height;
        for (int i = 0; i < numberOfYLabels; i++)
        {
            var label = Instantiate(yLabelPrefab, yLabelParent);
            yLabels.Add(label);

            float normalized = (float)i / (numberOfYLabels - 1);
            float y = normalized * height * yLabelSpacingFactor;

            label.rectTransform.anchoredPosition = new Vector2(yLabelOffset, y);
            label.text = (normalized * maxVelocityToShow).ToString("F1");
        }
    }

    void SetupXLabels()
    {
        float width = graphRect.rect.width;
        for (int i = 0; i < numberOfXLabels; i++)
        {
            var label = Instantiate(xLabelPrefab, xLabelParent);
            xLabels.Add(label);

            float normalized = (float)i / (numberOfXLabels - 1);
            float x = normalized * width * xLabelSpacingFactor + xLabelHorizontalOffset;

            label.rectTransform.anchoredPosition = new Vector2(x, xLabelOffset);
        }
    }

    void UpdateXLabels()
    {
        if (xLabels.Count != numberOfXLabels) return;

        float width = graphRect.rect.width;
        float baseTime = timeElapsed - graphDuration;
        if (baseTime < 0) baseTime = 0;

        for (int i = 0; i < xLabels.Count; i++)
        {
            float normalized = (float)i / (numberOfXLabels - 1);
            float labelTime = baseTime + normalized * graphDuration;
            xLabels[i].text = labelTime.ToString("F1") + "s";
        }
    }

    void UpdateYLabels()
    {
        float height = graphRect.rect.height;

        for (int i = 0; i < yLabels.Count; i++)
        {
            float normalized = (float)i / (numberOfYLabels - 1);
            float y = normalized * height;
            yLabels[i].rectTransform.anchoredPosition = new Vector2(yLabelOffset, y);
            yLabels[i].text = (normalized * maxVelocityToShow).ToString("F1");

        }
    }
    void OnEnable()
    {
        UpdateGraph();
        UpdateXLabels();
    }

}
