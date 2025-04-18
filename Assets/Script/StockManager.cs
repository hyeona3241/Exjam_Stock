using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class StockManager : MonoBehaviour
{
    public MarketTimer marketTimer;
    public List<StockData> allStocks;
    public GraphController graphController;

    void Start()
    {
        marketTimer.OnFiveMinutesPassed += SendDataToGraph;
        marketTimer.OnTenMinutesPassed += SaveStockPricesToCSV; // �� �� �� �־�� ��!
    }

    void SendDataToGraph()
    {
        foreach (var stock in allStocks)
        {
            //graphController.AddPoint(stock.stockName, stock.currentPrice, stock.lineColor);
            graphController.AddDataPoint(stock.stockName, marketTimer.ElapsedTime, stock.currentPrice, stock.lineColor, stock.lineColor);

        }
    }


    void SaveStockPricesToCSV()
    {
        string path = Application.dataPath + "/StockPrices.csv";

        bool fileExists = File.Exists(path);
        List<string> lines = new List<string>();

        if (fileExists)
        {
            lines.AddRange(File.ReadAllLines(path));
        }
        else
        {
            // ��� ����: StockName, Round1, Round2 ...
            StringBuilder header = new StringBuilder("StockName");
            foreach (var stock in allStocks)
            {
                header.Append($",{stock.stockName}");
            }
            lines.Add(header.ToString());
        }

        // ���ο� ���� �߰�
        StringBuilder newLine = new StringBuilder();
        string timeLabel = TimeSpan.FromSeconds(marketTimer.ElapsedTime).ToString(@"mm\:ss");
        newLine.Append(timeLabel); // ù �÷��� �ð�

        foreach (var stock in allStocks)
        {
            newLine.Append($",{stock.currentPrice:F2}");
        }

        lines.Add(newLine.ToString());
        File.WriteAllLines(path, lines.ToArray(), Encoding.UTF8);

        UnityEngine.Debug.Log("�ֽ� ������ CSV ���� �Ϸ�!");
    }
}
