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

    [Header("Graph Settings")]
    public float graphDuration = 5f;
    public float maxVelocityToShow = 10f;
    public int numberOfYLabels = 5;
    public int numberOfXLabels = 6;
    public float updateRate = 0.1f;

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
        }
    }

    void UpdateGraph()
    {
        graphRenderer.graphPoints.Clear();

        if (velocitySamples.Count < 2)
            return;

        float width = graphRect.rect.width;
        float height = graphRect.rect.height;
        float xScale = width / graphDuration;
        float yScale = height / maxVelocityToShow;

        float baseTime = timeSamples[0];

        for (int i = 0; i < velocitySamples.Count; i++)
        {
            float x = (timeSamples[i] - baseTime) * xScale;
            float y = Mathf.Clamp(velocitySamples[i] * yScale, 0, height);
            graphRenderer.graphPoints.Add(new Vector2(x, y));
        }

        graphRenderer.Redraw();
    }

    void SetupYLabels()
    {
        float height = graphRect.rect.height;
        for (int i = 0; i < numberOfYLabels; i++)
        {
            var label = Instantiate(yLabelPrefab, yLabelParent);
            yLabels.Add(label);

            float normalized = (float)i / (numberOfYLabels - 1);
            float y = normalized * height;

            label.rectTransform.anchoredPosition = new Vector2(-40, y);
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
            float x = normalized * width;

            label.rectTransform.anchoredPosition = new Vector2(x, -20);
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
    void OnEnable()
    {
        UpdateGraph();
        UpdateXLabels();
    }

}
