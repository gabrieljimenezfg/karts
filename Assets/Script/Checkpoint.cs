using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isGoal;
    public int checkpointIndex;
    // DEBUG
    public int previousCheckpointNeeded;

    private void Update()
    {
        previousCheckpointNeeded = GetPreviousCheckpoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarInfo>(out var car))
        {
            HandleCarInCheckpoint(car);
        }
    }

    private int GetPreviousCheckpoint()
    {
        if (checkpointIndex == 0)
        {
            return -1;
        }
        else
        {
            return checkpointIndex - 1;
        }
    }

    private void HandleCarInCheckpoint(CarInfo car)
    {
        if (car.checkpoint != GetPreviousCheckpoint()) return;

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