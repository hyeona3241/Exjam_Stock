using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    [Header("������")]
    public GameObject linePrefab;       // LineRenderer�� ���� ������
    public GameObject pointPrefab;      // ���� �� ������

    [Header("�׷��� ����")]
    public RectTransform graphContainer;

    [Header("�׷��� ����")]
    public float xSpacing = 10f;        // �ð� ������ x ����
    public float yScale = 1f;           // �ְ� ���� y ��ġ�� ��ȯ�� �� ������

    private List<Vector2> graphPoints = new List<Vector2>();
    private LineRenderer lineRenderer;
    private float currentTime = 0f;

    void Start()
    {
        // ���� ������ �ν��Ͻ� ����
        GameObject lineObj = Instantiate(linePrefab, graphContainer);
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    public void AddDataPoint(float price, Color lineColor, Color pointColor)
    {
        float x = currentTime * xSpacing;
        float y = price * yScale;
        Vector2 newPoint = new Vector2(x, y);
        graphPoints.Add(newPoint);

        // LineRenderer ����
        lineRenderer.positionCount = graphPoints.Count;
        for (int i = 0; i < graphPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, graphPoints[i]);
        }

        // ���� ����
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // �� ���� + ���� ����
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = newPoint;

        Image img = point.GetComponent<Image>();
        if (img != null)
            img.color = pointColor;

        currentTime += 1f;
    }

}
