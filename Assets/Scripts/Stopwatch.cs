using System;
using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour {
    bool stopwatchActive = false;
    float currentTime = 0;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI elapsedText;

    void Update() {
        if (stopwatchActive) {
            currentTime += Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = string.Format("{0:00}:{1:00}:{2:00}",
                  (int)(time.TotalHours),
                  time.Minutes,
                  time.Seconds);
    }

    public void StartStopwatch() {
        stopwatchActive = true;
    }

    public void StopStopwatch() {
        stopwatchActive = false;
    }

    public void ResetStopwatch() {
        currentTime = 0;
    }

    public void SetElapsedText(string text) {
        elapsedText.text = text;
    }
}
