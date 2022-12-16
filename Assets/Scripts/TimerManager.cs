using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float startTime = 0f;
    public float currentTime;
    public bool timerStarted = false;

    public TextMeshProUGUI textMeshPro;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (timerStarted == true) {
            currentTime += Time.deltaTime;

            textMeshPro.text = GetCurrentTime();
        }
    }

    public string GetCurrentTime() {
        int minutes = (int)currentTime/60;
        int seconds = (int)currentTime%60;

        return minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
    }

    public void StartTimer() {
        timerStarted = true;
    }
    public void ResetTimer() {
        currentTime = 0;
        timerStarted = false;
    }
    public void StopTimer() {
        timerStarted = false;
    }
}
