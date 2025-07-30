using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVelocityGraphRenderer : Graphic
{
    public List<Vector2> graphPoints = new List<Vector2>();
    public float lineWidth = 2f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Debug.Log($"[UIVelocityGraphRenderer] Redrawing mesh with {graphPoints.Count} points");
        vh.Clear();

        if (graphPoints.Count < 2)
        {
            Debug.LogWarning("[UIVelocityGraphRenderer] Not enough points to draw a line.");
            return;
        }
            

        for (int i = 0; i < graphPoints.Count - 1; i++)
        {
            Vector2 p0 = graphPoints[i];
            Vector2 p1 = graphPoints[i + 1];

            Vector2 direction = (p1 - p0).normalized;
            Vector2 normal = new Vector2(-direction.y, direction.x) * lineWidth * 0.5f;

            UIVertex v0 = UIVertex.simpleVert;
            UIVertex v1 = UIVertex.simpleVert;
            UIVertex v2 = UIVertex.simpleVert;
            UIVertex v3 = UIVertex.simpleVert;

            v0.position = p0 - normal;
            v1.position = p0 + normal;
            v2.position = p1 + normal;
            v3.position = p1 - normal;

            v0.color = color;
            v1.color = color;
            v2.color = color;
            v3.color = color;

            int startIndex = vh.currentVertCount;
            vh.AddVert(v0);
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }
    }

    public void Redraw()
    {
        SetVerticesDirty();
    }
}
