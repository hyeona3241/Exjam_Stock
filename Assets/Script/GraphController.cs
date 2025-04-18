using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject pointPrefab;
    public GameObject lineImagePrefab; // 선을 UI Image로 표시할 프리팹

    [Header("그래프 영역")]
    public RectTransform graphContainer;
    public ScrollRect scrollRect;

    [Header("그래프 설정")]
    public float xSpacing = 10f;
    public float yScale = 10f;
    public float xStartOffset = 0f;
    public float yStartOffset = 0f;

    private Dictionary<string, List<Vector2>> graphPoints = new();

    public void AddDataPoint(string stockName, float time, float price, Color lineColor, Color pointColor)
    {
        if (!graphPoints.ContainsKey(stockName))
        {
            graphPoints[stockName] = new List<Vector2>();
        }

        float x = -230f;
        x += xStartOffset + (time / 10f) * xSpacing;
        float y = yStartOffset + price * yScale;
        Vector2 newPoint = new Vector2(x, y);
        var points = graphPoints[stockName];

        // 선 그리기
        if (points.Count > 0)
        {
            Vector2 prevPoint = points[points.Count - 1];
            DrawUILine(prevPoint, newPoint, lineColor);
        }

        points.Add(newPoint);

        // 점 생성
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = newPoint;

        Image img = point.GetComponent<Image>();
        if (img != null)
            img.color = pointColor;

        // 3. Content 영역 확장
        graphContainer.sizeDelta = new Vector2(newPoint.x + 100f, graphContainer.sizeDelta.y);

        // (선택) 4. 자동 스크롤
        scrollRect.horizontalNormalizedPosition = 1f;  // <- 이건 자동스크롤, 원하면 주석 처리
    }

    private void DrawUILine(Vector2 start, Vector2 end, Color color)
    {
        GameObject lineObj = Instantiate(lineImagePrefab, graphContainer);
        RectTransform rt = lineObj.GetComponent<RectTransform>();

        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rt.sizeDelta = new Vector2(distance, 2f); // 선 두께 2
        rt.anchoredPosition = start;
        rt.pivot = new Vector2(0, 0.5f);
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        Image img = lineObj.GetComponent<Image>();
        if (img != null)
            img.color = color;

    }
}
