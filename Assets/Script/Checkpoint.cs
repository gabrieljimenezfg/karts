using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static event EventHandler<TimeSpan> PlayerClearedLap;

    [SerializeField] private bool isGoal;
    public int checkpointIndex;

    private List<TimeSpan> lapTimes = new List<TimeSpan>();
    private TimeSpan totalRaceTime;
    private TimeSpan[] sectorTimes;
    private DateTime raceStartTime;

    // DEBUG
    public int previousCheckpointNeeded;

    private void Update()
    {
        previousCheckpointNeeded = GetPreviousCheckpoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<CarInfo>(out var carInfo)) return;
        if (carInfo.checkpoint != GetPreviousCheckpoint()) return;

        if (other.TryGetComponent<CarController>(out _))
        {
            HandlePlayerThroughCheckpoint(carInfo.lap);
        }

        HandleCarInCheckpoint(carInfo);
    }

    private void HandlePlayerThroughCheckpoint(int currentCarLap)
    {
        if (isGoal)
        {
            if (currentCarLap == 0)
            {
                raceStartTime = DateTime.Now;
            }
            else
            {
                var totalLapsTime = TimeSpan.Zero;
                foreach (var lapTime in lapTimes)
                {
                    totalLapsTime += lapTime;
                }

                var currentLapTime = DateTime.Now - raceStartTime + totalLapsTime;
                lapTimes.Add(currentLapTime);
                PlayerClearedLap?.Invoke(this, currentLapTime);
            }
        }
    }

    private int GetPreviousCheckpoint()
    {
        if (checkpointIndex == 0)
        {
            return -1;
        }

        return checkpointIndex - 1;
    }

    private void HandleCarInCheckpoint(CarInfo car)
    {
        if (isGoal)
        {
            car.lap++;
            car.checkpoint = 0;
        }
        else
        {
            if (checkpointIndex == CheckpointManager.Instance.checkpoints.Length - 1)
            {
                car.checkpoint = -1;
            }
            else
            {
                car.checkpoint++;
            }
        }
    }
}