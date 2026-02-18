using System;
using UnityEngine;

public enum SensorPosition {
    Front,
    FrontLeft,
    FrontLeftDiagonal,
    FrontRight,
    FrontRightDiagonal,
}

public class Sensor : MonoBehaviour
{
    [SerializeField] public SensorPosition sensorPosition;
}
