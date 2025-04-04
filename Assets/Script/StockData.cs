using UnityEngine;
using UnityEngine.UI;

public class StockData : MonoBehaviour
{
    [Header("기본 정보")]
    public string stockName;
    public float initialPrice = 100f;
    public float fluctuationRate = 0.02f; // 2% 변화
    public float currentPrice;
    public Color lineColor; // 각 주식별 고유 색상

    [Header("UI")]
    public Text priceText;

    [Header("업데이트 설정")]
    public float updateInterval = 1f; // 1초마다 갱신

    void Start()
    {
        currentPrice = initialPrice;
        UpdatePriceText();

        // 1초마다 `UpdateStockPrice` 호출
        InvokeRepeating(nameof(UpdateStockPrice), updateInterval, updateInterval);
    }

    void UpdateStockPrice()
    {
        // 랜덤으로 상승/하락 결정
        float randomFactor = Random.Range(-1f, 1f);
        float change = currentPrice * fluctuationRate * randomFactor;
        currentPrice += change;

        UpdatePriceText();
    }

    void UpdatePriceText()
    {
        //priceText.text = stockName + ": " + currentPrice.ToString("F2") + "₩";
        priceText.text = currentPrice.ToString("F2") + " ₩";
    }
}
