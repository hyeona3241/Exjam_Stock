using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class MarketTimer : MonoBehaviour
{
    [Header("Ÿ�̸� UI")]
    public Text timerText;
    public GameObject pausedPanel;  // "PAUSED" �ؽ�Ʈ ������Ʈ (SetActive�� on/off)

    [Header("�ð� �帧")]
    public float elapsedTime = 0f;
    private float lastFiveMinMark = 0f;
    private float lastTenMinMark = 0f;
    public float ElapsedTime => elapsedTime;


    [Header("�Ͻ����� ����")]
    public bool isPaused = false;

    // �ð� �̺�Ʈ
    public event Action OnFiveMinutesPassed;
    public event Action OnTenMinutesPassed;

    void Update()
    {
        if (isPaused)
        {
            if (pausedPanel != null) pausedPanel.SetActive(true);
            return;
        }
        else
        {
            if (pausedPanel != null) pausedPanel.SetActive(false);
        }

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();

        //5�� = 300f
        if (elapsedTime - lastFiveMinMark >= 10f)
        {
            OnFiveMinutesPassed?.Invoke();
            lastFiveMinMark = elapsedTime;
        }

        if (elapsedTime - lastTenMinMark >= 10f)
        {
            OnTenMinutesPassed?.Invoke();
            lastTenMinMark = elapsedTime;
            UnityEngine.Debug.Log("[CSV ����] ���� �ֽ� ������ �����߽��ϴ�.");
        }
    }

    void UpdateTimerUI()
    {
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        lastFiveMinMark = 0f;
        lastTenMinMark = 0f;
        UpdateTimerUI();
    }
}
