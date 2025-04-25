using UnityEngine;
using UnityEngine.UI;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class MarketTimer : MonoBehaviour
{
    [Header("Ÿ�̸� UI")]
    public UnityEngine.UI.Text timerText;
    public GameObject pausedPanel;

    [Header("�˶� ����")]
    public AudioClip tenMinuteAlertClip;    // 10�� ��� �� ����� Ŭ��
    private AudioSource audioSource;

    [Header("�ð� �帧")]
    private float elapsedTime = 0f;
    private float lastFiveMinMark = 0f;
    private float lastTenMinMark = 0f;
    public float ElapsedTime => elapsedTime;

    [Header("�Ͻ����� ����")]
    public bool isPaused = false;

    public event Action OnFiveMinutesPassed;
    public event Action OnTenMinutesPassed;

    void Awake()
    {
        // AudioSource ����
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

        // 1�� üũ
        if (elapsedTime - lastFiveMinMark >= 120f)
        {
            OnFiveMinutesPassed?.Invoke();
            lastFiveMinMark = elapsedTime;
        }

        // 10�� üũ & ���� �˶�
        if (elapsedTime - lastTenMinMark >= 600f)
        {
            OnTenMinutesPassed?.Invoke();
            lastTenMinMark = elapsedTime;
            UnityEngine.Debug.Log("[CSV ����] ���� �ֽ� ������ �����߽��ϴ�.");

            //10�� �˶� ���
            if (tenMinuteAlertClip != null)
                audioSource.PlayOneShot(tenMinuteAlertClip);
            else
                UnityEngine.Debug.LogWarning("�˶��� AudioClip(tenMinuteAlertClip)�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void UpdateTimerUI()
    {
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        int totalMin = (int)time.TotalMinutes;    // ���� ��
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
