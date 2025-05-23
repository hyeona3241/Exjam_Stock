using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

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
            graphPoints[stockName] = new List<Vector2>();

        // 새 좌표 계산 (Pivot이 (0,0) 기준)
        float x = -300f;
        float y = -100f;
        x += xStartOffset + (time / 120f) * xSpacing;
        y += yStartOffset + (price * 10f) * yScale;
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

        UnityEngine.UI.Image img = point.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = pointColor;

        // 1) 뷰포트 크기 가져오기
        float vpW = scrollRect.viewport.rect.width;
        float vpH = scrollRect.viewport.rect.height;

        // 2) 콘텐츠 확장: 새 점 위치 + 뷰포트 크기만큼 확보
        Vector2 updatedSize = graphContainer.sizeDelta;
        if (newPoint.x + vpW > updatedSize.x)
            updatedSize.x = newPoint.x + vpW;
        if (newPoint.y + vpH > updatedSize.y)
            updatedSize.y = newPoint.y + vpH;
        graphContainer.sizeDelta = updatedSize;


        //현재 스크롤 위치를 유지시키기 위한 고정 (자동스크롤 안 쓸 경우)
        float currentX = scrollRect.horizontalNormalizedPosition;
        float currentY = scrollRect.verticalNormalizedPosition;


        // 콘텐츠 변경(크기 확장) 후 강제로 오른쪽·위쪽 끝으로 스크롤
        Canvas.ForceUpdateCanvases();
        //scrollRect.horizontalNormalizedPosition = 1f;  // 오른쪽 끝
        //scrollRect.verticalNormalizedPosition = 1f;  // 위쪽 끝

        // 이 코드는 영역 확장 이후에 실행되어야 함
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
        
        rt.sizeDelta = new Vector2(distance, 2f); // 선 두께
        rt.anchoredPosition = start;
        rt.pivot = new Vector2(0, 0.5f);
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        UnityEngine.UI.Image img = lineObj.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = color;
    }

}
