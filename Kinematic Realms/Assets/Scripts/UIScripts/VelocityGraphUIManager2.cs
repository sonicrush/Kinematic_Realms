using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class VelocityGraphUIManager2 : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public UIDocument uiDocument; // UI Toolkit UIDocument reference
    public TextMeshProUGUI xLabelPrefab;
    public TextMeshProUGUI yLabelPrefab;
    public RectTransform xLabelParent;
    public RectTransform yLabelParent;

    [Header("Graph Settings")]
    public float graphDuration = 5f;
    public float maxVelocityToShow = 10f;
    public float updateRate = 0.1f;

    [Header("Y Axis Labels")]
    public int numberOfYLabels = 5;
    public float yLabelSpacingFactor = 1.0f;
    public float yLabelOffset = -40f;

    [Header("X Axis Labels")]
    public int numberOfXLabels = 6;
    public float xLabelSpacingFactor = 1.0f;
    public float xLabelOffset = -20f;
    public float xLabelHorizontalOffset = 0f;

    private List<float> timeSamples = new();
    private List<float> velocitySamples = new();
    private float timeElapsed = 0f;
    private float updateTimer = 0f;
    private Vector3 lastPosition;

    private List<TextMeshProUGUI> xLabels = new();
    private List<TextMeshProUGUI> yLabels = new();

    private VelocityGraphElement graphElement;

    void Start()
    {
        lastPosition = target.position;

        var root = uiDocument.rootVisualElement;
        var graphContainer = root.Q<VisualElement>("GraphContainer");
        graphElement = new VelocityGraphElement();
        graphElement.style.flexGrow = 1;
        graphContainer.Add(graphElement);

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
        if (graphElement.resolvedStyle.width <= 0f || graphElement.resolvedStyle.height <= 0f)
            return;

        float width = graphElement.resolvedStyle.width;
        float height = graphElement.resolvedStyle.height;
        float xScale = width / graphDuration;
        float yScale = height / maxVelocityToShow;

        graphElement.graphPoints.Clear();

        if (timeSamples.Count == 0 || velocitySamples.Count == 0)
            return;

        float baseTime = timeSamples[0];

        for (int i = 0; i < velocitySamples.Count; i++)
        {
            float timeOffset = timeSamples[i] - baseTime;
            float x = timeOffset * xScale;
            float y = height - velocitySamples[i] * yScale;

            graphElement.graphPoints.Add(new Vector2(x, y));
        }

        graphElement.Redraw();
    }

    void SetupYLabels()
    {
        float height = graphElement.resolvedStyle.height;
        yLabelOffset = 0f;
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
        float width = graphElement.resolvedStyle.width;
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
        float height = graphElement.resolvedStyle.height;
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
        graphElement.RegisterCallback<GeometryChangedEvent>((evt) =>
        {
            SetupYLabels();
            SetupXLabels();
            UpdateGraph();
            UpdateXLabels();
        });
    }
}
