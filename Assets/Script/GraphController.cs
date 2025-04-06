using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    [Header("������")]
    public GameObject linePrefab;
    public GameObject pointPrefab;

    [Header("�׷��� ����")]
    public RectTransform graphContainer;

    [Header("�׷��� ����")]
    public float xSpacing = 10f;
    public float yScale = 10f;

    private Dictionary<string, LineRenderer> lineRenderers = new();
    private Dictionary<string, List<Vector2>> graphPoints = new();

    public void AddDataPoint(string stockName, float time, float price, Color lineColor, Color pointColor)
    {
        if (!lineRenderers.ContainsKey(stockName))
        {
            // ���ο� ���� �߰�
            GameObject lineObj = Instantiate(linePrefab, graphContainer);
            LineRenderer lr = lineObj.GetComponent<LineRenderer>();
            lr.positionCount = 0;
            lr.startColor = lineColor;
            lr.endColor = lineColor;

            lineRenderers[stockName] = lr;
            graphPoints[stockName] = new List<Vector2>();
        }

        float x = -300.0f;
        float y = -100.0f;
        x += (time/10.0f) * xSpacing;
        y += price * yScale;
        Vector2 newPoint = new Vector2(x, y);
        graphPoints[stockName].Add(newPoint);

        // �� ����
        LineRenderer lineRenderer = lineRenderers[stockName];
        lineRenderer.positionCount = graphPoints[stockName].Count;
        for (int i = 0; i < graphPoints[stockName].Count; i++)
        {
            lineRenderer.SetPosition(i, graphPoints[stockName][i]);
        }

        // �� ����
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = newPoint;

        UnityEngine.UI.Image img = point.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.color = pointColor;
        }
    }
}
