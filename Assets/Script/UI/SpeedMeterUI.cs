using System;
using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;

public class SpeedMeterUI : MonoBehaviour
{
    [SerializeField] private Transform needle;
    [SerializeField] private TextMeshProUGUI speedText;
    private const float minimumRotation = 36;
    private const float maximumRotation = -197;
    private CarController player;

    private float maxKmH;

    private void Start()
    {
        player = CarController.Instance;
        maxKmH = player.MaxSpeed;
    }

    private void Update()
    {
        var playerSpeed = player.GetCurrentSpeedKmH();
        SetNeedleRotation(playerSpeed);
        speedText.text = playerSpeed.ToString("000");
    }

    private void SetNeedleRotation(float playerSpeed)
    {
        var progress = playerSpeed / maxKmH;
        Debug.Log(progress);
        var currentNeedleRotation = Mathf.Lerp(minimumRotation, maximumRotation, progress);
        needle.localEulerAngles = new Vector3(0, 0, currentNeedleRotation);
    }
}