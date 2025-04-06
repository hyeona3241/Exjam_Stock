using System;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StockData : MonoBehaviour
{
    [Header("기본 정보")]
    public string stockName;
    public float initialPrice = 100f;
    public float fluctuationRate = 0.02f;
    public float currentPrice;
    public Color lineColor;

    [Header("UI")]
    public UnityEngine.UI.Text priceText;

    [Header("업데이트 설정")]
    public float updateInterval = 1f;

    private float updateTimer = 0f;
    private MarketTimer marketTimer;

    void Start()
    {
        currentPrice = initialPrice;
        UpdatePriceText();

        marketTimer = FindObjectOfType<MarketTimer>();
    }

    void Update()
    {
        if (marketTimer != null && marketTimer.isPaused) return;

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            UpdateStockPrice();
            updateTimer = 0f;
        }
    }

    void UpdateStockPrice()
    {
        float randomFactor = UnityEngine.Random.Range(-1f, 1f);
        float change = currentPrice * fluctuationRate * randomFactor;
        currentPrice += change;

        UpdatePriceText();
    }

    void UpdatePriceText()
    {
        priceText.text = currentPrice.ToString("F2") + " ₩";
    }
}
