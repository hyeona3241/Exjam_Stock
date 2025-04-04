using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphController : MonoBehaviour
{
    [Header("프리팹")]
    public GameObject linePrefab;       // LineRenderer가 붙은 프리팹
    public GameObject pointPrefab;      // 원형 점 프리팹

    [Header("그래프 영역")]
    public RectTransform graphContainer;

    [Header("그래프 설정")]
    public float xSpacing = 10f;        // 시간 단위당 x 간격
    public float yScale = 1f;           // 주가 값을 y 위치로 변환할 때 스케일

    private List<Vector2> graphPoints = new List<Vector2>();
    private LineRenderer lineRenderer;
    private float currentTime = 0f;

    void Start()
    {
        // 라인 프리팹 인스턴스 생성
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

        // LineRenderer 갱신
        lineRenderer.positionCount = graphPoints.Count;
        for (int i = 0; i < graphPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, graphPoints[i]);
        }

        // 색상 적용
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // 점 생성 + 색상 지정
        GameObject point = Instantiate(pointPrefab, graphContainer);
        RectTransform rt = point.GetComponent<RectTransform>();
        rt.anchoredPosition = newPoint;

        Image img = point.GetComponent<Image>();
        if (img != null)
            img.color = pointColor;

        currentTime += 1f;
    }

}
