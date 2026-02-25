using System;
using System.Collections.Generic;
using UnityEngine;

public class CarInfo : MonoBehaviour
{
    private struct ScaleFactor
    {
        public static readonly int Laps = 1000;
        public static readonly int Checkpoint = 100;
    }

    public int lap;
    public int checkpoint;

    public float OrderReference()
    {
        var distance = transform.position - CheckpointManager.Instance.checkpoints[checkpoint + 1].transform.position;
        return (lap * ScaleFactor.Laps) + (checkpoint * ScaleFactor.Checkpoint) + distance.magnitude;
    }
}