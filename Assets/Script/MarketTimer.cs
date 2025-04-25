using UnityEngine;
using UnityEngine.UI;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class MarketTimer : MonoBehaviour
{
    [Header("타이머 UI")]
    public UnityEngine.UI.Text timerText;
    public GameObject pausedPanel;

    [Header("알람 설정")]
    public AudioClip tenMinuteAlertClip;    // 10분 경과 시 재생할 클립
    private AudioSource audioSource;

    [Header("시간 흐름")]
    private float elapsedTime = 0f;
    private float lastFiveMinMark = 0f;
    private float lastTenMinMark = 0f;
    public float ElapsedTime => elapsedTime;

    [Header("일시정지 상태")]
    public bool isPaused = false;

    public event Action OnFiveMinutesPassed;
    public event Action OnTenMinutesPassed;

    void Awake()
    {
        // AudioSource 세팅
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isPaused)
        {
            if (pausedPanel != null) pausedPanel.SetActive(true);
            return;
        }
        else if (pausedPanel != null)
        {
            pausedPanel.SetActive(false);
        }

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();

        // 1분 체크
        if (elapsedTime - lastFiveMinMark >= 120f)
        {
            OnFiveMinutesPassed?.Invoke();
            lastFiveMinMark = elapsedTime;
        }

        // 10분 체크 & 저장 알람
        if (elapsedTime - lastTenMinMark >= 600f)
        {
            OnTenMinutesPassed?.Invoke();
            lastTenMinMark = elapsedTime;
            UnityEngine.Debug.Log("[CSV 저장] 현재 주식 가격을 저장했습니다.");

            //10분 알람 재생
            if (tenMinuteAlertClip != null)
                audioSource.PlayOneShot(tenMinuteAlertClip);
            else
                UnityEngine.Debug.LogWarning("알람용 AudioClip(tenMinuteAlertClip)이 할당되지 않았습니다.");
        }
    }

    void UpdateTimerUI()
    {
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        int totalMin = (int)time.TotalMinutes;    // 누적 분
        int seconds = time.Seconds;

        timerText.text = string.Format("{0:D2}:{1:D2}", totalMin, seconds);
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
