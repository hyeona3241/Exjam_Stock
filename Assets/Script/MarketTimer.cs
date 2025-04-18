using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class MarketTimer : MonoBehaviour
{
    [Header("타이머 UI")]
    public Text timerText;
    public GameObject pausedPanel;  // "PAUSED" 텍스트 오브젝트 (SetActive로 on/off)

    [Header("시간 흐름")]
    public float elapsedTime = 0f;
    private float lastFiveMinMark = 0f;
    private float lastTenMinMark = 0f;
    public float ElapsedTime => elapsedTime;


    [Header("일시정지 상태")]
    public bool isPaused = false;

    // 시간 이벤트
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

        //5분 = 300f
        if (elapsedTime - lastFiveMinMark >= 10f)
        {
            OnFiveMinutesPassed?.Invoke();
            lastFiveMinMark = elapsedTime;
        }

        if (elapsedTime - lastTenMinMark >= 10f)
        {
            OnTenMinutesPassed?.Invoke();
            lastTenMinMark = elapsedTime;
            UnityEngine.Debug.Log("[CSV 저장] 현재 주식 가격을 저장했습니다.");
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
