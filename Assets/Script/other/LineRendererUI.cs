
using UnityEngine;


public sealed class LineRendererUI : MonoBehaviour
{
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    public void DrawLine(Vector3[] positions)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        Invoke("endLine", 0.2f);
    }
    void endLine()
    {
        lineRenderer.enabled = false;
    }

}

