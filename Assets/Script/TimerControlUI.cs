using UnityEngine;

public class TimerControlUI : MonoBehaviour
{
    public MarketTimer marketTimer;

    public void OnPauseButtonClicked()
    {
        marketTimer.Pause();
    }

    public void OnResumeButtonClicked()
    {
        marketTimer.Resume();
    }

    public void OnResetButtonClicked()
    {
        marketTimer.ResetTimer();
    }
}
