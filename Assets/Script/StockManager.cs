using System.Collections.Generic;
using UnityEngine;

public class StockManager : MonoBehaviour
{
    public MarketTimer marketTimer;
    public List<StockData> allStocks;
    public GraphController graphController;

    void Start()
    {
        marketTimer.OnFiveMinutesPassed += SendDataToGraph;
    }

    void SendDataToGraph()
    {
        foreach (var stock in allStocks)
        {
            //graphController.AddPoint(stock.stockName, stock.currentPrice, stock.lineColor);
            // 예: StockData에서 매초 가격을 받아서 전달
            graphController.AddDataPoint(stock.currentPrice, stock.lineColor, stock.lineColor);

        }
    }
}
