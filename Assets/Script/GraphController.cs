using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GraphController : MonoBehaviour
{
    [Header("������")]
    public GameObject pointPrefab;
    public GameObject lineImagePrefab; // ���� UI Image�� ǥ���� ������

    [Header("�׷��� ����")]
    public RectTransform graphContainer;
    public ScrollRect scrollRect;

    [Header("�׷��� ����")]
    public float xSpacing = 10f;
    public float yScale = 10f;
    public float xStartOffset = 0f;
    public float yStartOffset = 0f;

    private Dictionary<string, List<Vector2>> graphPoints = new();

    public void AddDataPoint(string stockName, float time, float price, Color lineColor, Color pointColor)
    {
        if (!graphPoints.ContainsKey(stockName))
            graphPoints[stockName] = new List<Vector2>();

        // �� ��ǥ ��� (Pivot�� (0,0) ����)
        float x = -300f;
        float y = -100f;
        x += xStartOffset + (time / 10f) * xSpacing;
        y += yStartOffset + price * yScale;
        Vector2 newPoint = new Vector2(x, y);
        var points = graphPoints[stockName];

        // �� �׸���
        if (points.Count > 0)
        {
            Vector2 prevPoint = points[points.Count - 1];
            DrawUILine(prevPoint, newPoint, lineColor);
        }

        points.Add(newPoint);

        // �� ����
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = newPoint;

        UnityEngine.UI.Image img = point.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = pointColor;

        // �׷��� ����(Content) ũ�� �ڵ� Ȯ��
        Vector2 updatedSize = graphContainer.sizeDelta;
        if (newPoint.x + 100f > updatedSize.x)
            updatedSize.x = newPoint.x + 100f;
        if (newPoint.y + 100f > updatedSize.y)
            updatedSize.y = newPoint.y + 100f;
        graphContainer.sizeDelta = updatedSize;

        //���� ��ũ�� ��ġ�� ������Ű�� ���� ���� (�ڵ���ũ�� �� �� ���)
        float currentX = scrollRect.horizontalNormalizedPosition;
        float currentY = scrollRect.verticalNormalizedPosition;

        // �� �ڵ�� ���� Ȯ�� ���Ŀ� ����Ǿ�� ��
        scrollRect.horizontalNormalizedPosition = currentX;
        scrollRect.verticalNormalizedPosition = currentY;

    }



    private void DrawUILine(Vector2 start, Vector2 end, Color color)
    {
        GameObject lineObj = Instantiate(lineImagePrefab, graphContainer);
        RectTransform rt = lineObj.GetComponent<RectTransform>();

        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rt.sizeDelta = new Vector2(distance, 2f); // �� �β�
        rt.anchoredPosition = start;
        rt.pivot = new Vector2(0, 0.5f);
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        UnityEngine.UI.Image img = lineObj.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = color;
    }

}
