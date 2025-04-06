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
            graphController.AddDataPoint(stock.stockName, marketTimer.ElapsedTime, stock.currentPrice, stock.lineColor, stock.lineColor);



        }
    }
}
