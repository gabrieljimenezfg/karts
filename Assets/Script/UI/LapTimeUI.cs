using System;
using TMPro;
using UnityEngine;

public class LapTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapTimeText;

    private void Start()
    {
        Checkpoint.PlayerClearedLap += OnPlayerClearedLap;
    }

    private void OnPlayerClearedLap(object sender, TimeSpan e)
    {
        ShowLapTime(e);
    }

    private void ShowLapTime(TimeSpan time)
    {
        lapTimeText.text = $"{time.Minutes:00}:{time.Seconds:00}:{time.Milliseconds / 10:00}";
    }
}