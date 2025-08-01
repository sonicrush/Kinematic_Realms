using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class VelocityGraphElement : VisualElement
{
    public List<Vector2> graphPoints = new();
    public Color graphColor = Color.green;
    public float lineWidth = 2f;

    public VelocityGraphElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private void OnGenerateVisualContent(MeshGenerationContext context)
    {
        if (graphPoints.Count < 2) return;

        var painter = context.painter2D;
        painter.strokeColor = graphColor;
        painter.lineWidth = lineWidth;

        painter.BeginPath();
        painter.MoveTo(graphPoints[0]);

        for (int i = 1; i < graphPoints.Count; i++)
        {
            painter.LineTo(graphPoints[i]);
        }

        painter.Stroke();
    }

    public void Redraw()
    {
        MarkDirtyRepaint();
    }
}
